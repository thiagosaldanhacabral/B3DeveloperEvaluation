using B3DeveloperEvaluation.Application.Interfaces;
using B3DeveloperEvaluation.Domain.Entities;

namespace B3DeveloperEvaluation.Application.Services;

public class CalcInvestmentService : ICalcInvestmentService
{
    private static readonly List<TaxBracket> _brackets =
        [
            new TaxBracket { UpToMonths = 6, Rate = 0.225m },
            new TaxBracket { UpToMonths = 12, Rate = 0.2m },
            new TaxBracket { UpToMonths = 24, Rate = 0.175m },
            new TaxBracket { UpToMonths = int.MaxValue, Rate = 0.15m }
        ];

    /// <summary>
    /// Calculates the gross value of an investment using a fixed monthly rate.
    /// </summary>
    public decimal CalculateGross(Investment investment)
    {
        ArgumentNullException.ThrowIfNull(investment);
        const decimal monthlyRate = 0.005m;
        return investment.Amount * (decimal)Math.Pow(1.0 + (double)monthlyRate, investment.Months);
    }

    /// <summary>
    /// Calculates the net value of an investment after applying the tax bracket.
    /// </summary>
    public decimal CalculateNet(Investment investment)
    {
        ArgumentNullException.ThrowIfNull(investment);
        var gross = CalculateGross(investment);
        var profit = gross - investment.Amount;
        var bracket = _brackets.First(b => investment.Months <= b.UpToMonths);
        return profit * (1 - bracket.Rate) + investment.Amount;
    }
}