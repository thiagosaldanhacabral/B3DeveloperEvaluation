using B3DeveloperEvaluation.Application.Dtos;

namespace B3DeveloperEvaluation.Application.Interfaces;

public interface ICalcInvestmentService
{
    InvestmentResponseDto CalculateReturn(decimal initialAmount, int months);
}
