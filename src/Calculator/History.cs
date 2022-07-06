using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    partial class History
    {
        static Calculations calc = new Calculations();

        string lastOp = string.Empty;
        string lastOperand = string.Empty;
        public string equation = string.Empty;
        bool wasWrapped = false;

        public enum State { operatorNow, operandNow };
        State state = State.operandNow;

        public void addOperand(double operand)
        {
            if (state != State.operandNow) redactEquation(lastOp);
            state = State.operandNow;
            lastOperand = Convert.ToString(operand);
        }

        public void setLastOperand(double operand)
        {
            lastOperand = Convert.ToString(operand);
        }

        public void setLastOperator(CheckedOp op)
        {
            if(state != State.operatorNow) redactEquation(lastOperand);
            state = State.operatorNow;
            switch (op)
            {
                case CheckedOp.nul:
                    break;
                case CheckedOp.add:
                    lastOp = "+";
                    break;
                case CheckedOp.sub:
                    lastOp = "-";
                    break;
                case CheckedOp.mul:
                    lastOp = "*";
                    break;
                case CheckedOp.div:
                    lastOp = "/";
                    break;
                default: break;
            }
        }

        public void wrapLastOperand(OperationCode operCode)
        {
            wasWrapped = true;
            switch (operCode)
            {
                case OperationCode.sqrt:
                    lastOperand = "sqrt(" + lastOperand + ")";
                    break;
                case OperationCode.negate:
                    lastOperand = "negate(" + lastOperand + ")";
                    break;
                case OperationCode.reciproc:
                    lastOperand = "reciproc(" + lastOperand + ")";
                    break;
                default: break;
            }
        }

        public void clearEquation()
        {
            equation = string.Empty;
            lastOp = string.Empty;
            lastOperand = string.Empty;
            State state = State.operandNow;
        }

        public void redactEquation(string elem)
        {
            equation += " " + elem;
        }

        public string getCraftedEquation()
        {
            if (state == State.operatorNow)
                return equation + " " + lastOp;
            else {
                return equation + " " + lastOperand;
            }
        }
    }
}
