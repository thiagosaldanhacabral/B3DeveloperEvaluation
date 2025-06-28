using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace B3DeveloperEvaluation.Application.Services;

public class CalcInvestmentService(IOptions<InvestmentRatesOptions> options, ILogger<CalcInvestmentService> logger) : ICalcInvestmentService
{
    private readonly decimal _cdi = options.Value.Cdi;
    private readonly decimal _tb = options.Value.Tb;
    private readonly ILogger<CalcInvestmentService> _logger = logger;

    /// <summary>
    /// Calculates the tax on profit according to the investment period.
    /// </summary>
    private static decimal CalculateTax(decimal profit, int months)
    {
        decimal rate = months switch
        {
            <= 6 => 0.225m,
            <= 12 => 0.20m,
            <= 24 => 0.175m,
            _ => 0.15m
        };
        return profit * rate;
    }

    /// <summary>
    /// Calculates the investment return after taxes.
    /// </summary>
    public InvestmentResponseDto CalculateReturn(decimal initialAmount, int months)
    {
        _logger.LogInformation("Calculating return for Amount={Amount}, Months={Months}", initialAmount, months);

        if (initialAmount <= 0 || months <= 0)
            throw new ArgumentException("Initial amount and months must be greater than zero.");

        decimal finalAmount = initialAmount;
        decimal monthlyRate = _cdi * _tb;

        for (int i = 0; i < months; i++)
            finalAmount *= (1 + monthlyRate);

        decimal grossAmount = finalAmount;
        decimal profit = grossAmount - initialAmount;
        decimal tax = CalculateTax(profit, months);
        decimal netAmount = grossAmount - tax;

        return new InvestmentResponseDto
        {
            GrossAmount = decimal.Round(grossAmount, 2),
            NetAmount = decimal.Round(netAmount, 2),
            TaxAmount = decimal.Round(tax, 2)
        };
    }
}