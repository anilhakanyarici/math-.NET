using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    interface IOperable<TOperand> 
    {
        TOperand Add(TOperand operand);
        TOperand Sub(TOperand operand);
        TOperand Mul(TOperand operand);
        string ToString();
    }
}
