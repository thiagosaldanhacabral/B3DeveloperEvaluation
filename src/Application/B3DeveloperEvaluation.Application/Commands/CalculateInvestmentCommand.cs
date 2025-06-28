using B3DeveloperEvaluation.Application.Dtos;
using MediatR;

namespace B3DeveloperEvaluation.Application.Commands
{
    public record CalculateInvestmentCommand(decimal Amount, int Months) : IRequest<InvestmentResponseDto>;
}
