﻿using B3DeveloperEvaluation.Application.Commands;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Application.Interfaces;
using MediatR;

namespace B3DeveloperEvaluation.Application.Handlers;

public class CalculateInvestmentCommandHandler(ICalcInvestmentService calcService) : IRequestHandler<CalculateInvestmentCommand, InvestmentResponseDto>
{
    private readonly ICalcInvestmentService _calcService = calcService;

    public Task<InvestmentResponseDto> Handle(CalculateInvestmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var investment = new Domain.Entities.Investment
            {
                Amount = request.Amount,
                Months = request.Months
            };

            var response = _calcService.CalculateReturn(investment.Amount, investment.Months);

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            throw new InvalidOperationException("An error occurred while calculating the investment return.", ex);
        }
    }
}