using B3DeveloperEvaluation.Application.Commands;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using MediatR;

namespace B3DeveloperEvaluation.Application.Handlers;

public class CalculateInvestmentCommandHandler(ICalcInvestmentService calcService) : IRequestHandler<CalculateInvestmentCommand, InvestmentResponseDto>
{
    private readonly ICalcInvestmentService _calcService = calcService;

    public Task<InvestmentResponseDto> Handle(CalculateInvestmentCommand request, CancellationToken cancellationToken)
    {
        var investment = new Domain.Entities.Investment
        {
            Amount = request.Amount,
            Months = request.Months
        };

        var gross = _calcService.CalculateGross(investment);
        var net = _calcService.CalculateNet(investment);

        var response = new InvestmentResponseDto
        {
            Gross = gross,
            Net = net
        };

        return Task.FromResult(response);
    }
}