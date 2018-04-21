using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    interface AllowToGenerics<TGenericClass, ThisType> : IDivisible<ThisType>, IComparable<ThisType>
    {
        ThisType Add(ThisType operand);
        ThisType Sub(ThisType operand);
        ThisType Mul(ThisType operand);
        ThisType Div(ThisType operand);
        string ToString();
        int CompareTo(ThisType operand);
    }
}
