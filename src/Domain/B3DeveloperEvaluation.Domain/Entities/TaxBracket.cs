using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DeveloperEvaluation.Domain.Entities
{
    public class TaxBracket
    {
        public int UpToMonths { get; set; }
        public decimal Rate { get; set; }
    }
}
