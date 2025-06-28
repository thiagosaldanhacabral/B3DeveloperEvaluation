using B3DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DeveloperEvaluation.Application.Interfaces
{
    public interface ICalcInvestmentService
    {
        decimal CalculateGross(Investment investment);
        decimal CalculateNet(Investment investment);
    }
}
