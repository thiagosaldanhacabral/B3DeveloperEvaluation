using B3DeveloperEvaluation.Domain.Entities;

namespace B3DeveloperEvaluation.Application.Interfaces
{
    public interface ICalcInvestmentService
    {
        decimal CalculateGross(Investment investment);
        decimal CalculateNet(Investment investment);
    }
}
