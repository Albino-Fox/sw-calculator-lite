using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public enum ERRORS { IsOK, DivZeroByZero, DivByZero, Overflow, SqrtOfNeg, InvalidInput };
    public enum OperationCode { sqrt, negate, reciproc };
    public enum State { firstOp, nextOp };
    public enum CheckedOp { nul, add, sub, mul, div };
}
