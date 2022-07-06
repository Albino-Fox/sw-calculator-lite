using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    partial class Calculations
    {
        public History history = new History();

        public bool isFirstInput { get; private set; } = true;
        public double accumulator { get; private set; } = 0;
        public double operand = 0;
        //public char lastOperation;

        public CheckedOp checkedOp = CheckedOp.nul;
        public State state = State.firstOp;
        public bool wasOperatorPressed = false;
        public bool isFirstEntry = true;

        public double bufferNumber = 0;

        public string numberText { get; private set; } = string.Empty;

        public void ifFirstEntry()
        {
            if (isFirstEntry)
            {
                accumulator = Double.Parse(numberText);


                history.addOperand(Double.Parse(numberText));
                history.setLastOperator(checkedOp);

                isFirstEntry = false;
            }
        }

        public void pushToNumber(char ch) //ch - digit or floating point
        {
            if (state == State.firstOp && wasOperatorPressed) {
                state = State.nextOp;
                isResult = false;
            }
            if (isFirstInput)
            {
                if (isCalcSequence) {
                    numberText = "0";
                    isResult = false;
                }
                if (ch != '0')
                {
                    if (ch != '.')
                        numberText = string.Empty;
                    isFirstInput = false;
                    numberText += ch;
                }
            }
            else
                numberText += ch;
        }
        public void resetNumberText()
        {
            isFirstInput = true;

            isResult = false;
            numberText = "0";
            if (isCalcSequence)
            {
                accumulator = 0;
            }
        }

        public int checkOp(CheckedOp op)
        {
            int status = 0;
            if (checkedOp != op)
            {
               status = doOperation(checkedOp);
            }
            checkedOp = (CheckedOp)op;
            return status;
        }

        public void doRoutine(int mode)
        {
            switch (mode)
            {
                case 0:


                    wasOperatorPressed = true;
                    numberText = "0";
                    isCalcSequence = false;
                    isFirstInput = true;
                    break;
                case 1:
                    history.addOperand(operand);

                    numberText = "0";
                    isFirstInput = true;
                    isCalcSequence = false;
                    isResult = true;
                    state = State.firstOp;
                    break;
                default: break;
            }
        }

        public void resetEquation()
        {
            isFirstEntry = true;
            isFirstInput = true;
            state = State.firstOp;
            numberText = "0";
            accumulator = 0;
            isResult = false;
            wasOperatorPressed = false;
            checkedOp = CheckedOp.nul;
            history.clearEquation();
        }

        public int doOperation(CheckedOp execute)
        {
            switch ((int)execute)
            {
                case 0: return 0;
                case 1: add(); return 0;
                case 2: subtract(); return 0;
                case 3: multiply(); return 0;
                case 4: return divide();
                    default: return 5;
            }
        }
        public int add()
        {
            int status = checkOp(CheckedOp.add);
            if (status != 0) return status;
            ifFirstEntry();
            history.setLastOperator(checkedOp);
            if (state == State.firstOp)
                doRoutine(0);
            else
            {
                if (numberText == string.Empty) numberText = "0";

                operand = Double.Parse(numberText);
                accumulator += operand;

                doRoutine(1);
            }
            return 0;
        }

        public int subtract()
        {
            int status = checkOp(CheckedOp.sub);
            if (status != 0) return status;
            ifFirstEntry();
            history.setLastOperator(checkedOp);
            if (state == State.firstOp)
                doRoutine(0);
            else
            {
                if (numberText == string.Empty) numberText = "0";

                operand = Double.Parse(numberText);
                accumulator -= operand;

                doRoutine(1);
            }
            return 0;
        }

        public int multiply()
        {
            int status = checkOp(CheckedOp.mul);
            if (status != 0) return status;
            ifFirstEntry();
            history.setLastOperator(checkedOp);
            if (state == State.firstOp)
                doRoutine(0);
            else
            {
                if (numberText == string.Empty)
                {
                    operand = 1;
                    numberText = "0";
                }
                else
                    operand = Double.Parse(numberText);

                accumulator *= operand;

                doRoutine(1);
            }
            return 0;
        }

        public int divide()
        {
            int status = checkOp(CheckedOp.div);
            if (status != 0) return status;
            ifFirstEntry();
            history.setLastOperator(checkedOp);
            if (state == State.firstOp)
                doRoutine(0);
            else
            {
                operand = Double.Parse(numberText);
                if (accumulator == 0 && operand == 0)
                    return (int)ERRORS.DivZeroByZero;
                if (operand == 0)
                    return (int)ERRORS.DivByZero;

                accumulator /= operand;

                doRoutine(1);
            }
            return (int)ERRORS.IsOK;
        }

        
        public int sqrt()
        {
            if (Double.Parse(numberText) < 0) return (int)ERRORS.SqrtOfNeg;
            numberText = Convert.ToString(Math.Sqrt(Double.Parse(numberText)));
            isFirstInput = true;
            isResult = true;

            if (numberText.Length >= 16) allowToWrite = true;
            else
                allowToWrite = false;
            return (int)ERRORS.IsOK;
        }

        public int reciproc()
        {
            if (Double.Parse(numberText) == 0) return (int)ERRORS.DivByZero;
            numberText = Convert.ToString(1.0 / Double.Parse(numberText));

            isFirstInput = true;
            isResult = true;

            if (numberText.Length >= 16) allowToWrite = true;
            else
                allowToWrite = false;
            return (int)ERRORS.IsOK;
        }

        public void signInverse()
        {
            if (state == State.firstOp && wasOperatorPressed)
            {
                state = State.nextOp;
                isResult = true;

                numberText = Convert.ToString(accumulator);
                if (Double.Parse(numberText) < 0) numberText = numberText.Remove(0, 1);
                else numberText = '-' + numberText;
            }
            else
            {
                if (numberText == "0") return;
                if (Double.Parse(numberText) < 0) numberText = numberText.Remove(0, 1);
                else numberText = '-' + numberText;
            }
            if (isCalcSequence)
                accumulator *= -1;
        }

        public bool isResult = false;
        public void backspace()
        {
            if (!isResult)
            {
                int offset = 0;
                if (numberText[0] == '-') offset = 1;

                if (numberText.Length <= offset + 2 && numberText[offset + 0] == '0')
                    isFirstInput = true;
                if (numberText.Length == offset + 1)
                {
                    numberText = "0";
                    isFirstInput = true;
                }
                else { 
                    numberText = numberText.Remove(numberText.Length - 1, 1); 
                    if(offset > 0 && numberText == "-0") numberText = "0";
                }
            }
        }

        public void percent()
        {
            numberText = Convert.ToString(Double.Parse(numberText) * (accumulator / 100.0));
            isFirstInput = true;
            isResult = true;
            if (numberText.Length >= 16) allowToWrite = true;
            else
                allowToWrite = false;
        }

        public void addToMemory()
        {
            bufferNumber += Double.Parse(numberText);
            isFirstInput = true;
        }

        public void subFromMemory()
        {
            bufferNumber -= Double.Parse(numberText);
            isFirstInput = true;
        }

        public void setMemory()
        {
            bufferNumber = Double.Parse(numberText);
            isFirstInput = true;
        }

        public void clearMemory()
        {
            bufferNumber = 0;
            isFirstInput = true;
        }

        public void extractMemory()
        {
            numberText = Convert.ToString(bufferNumber);
            operand = Double.Parse(numberText);
            if (state == State.firstOp && wasOperatorPressed)
            {
                state = State.nextOp;
            }
            isFirstInput = true;
        }



        public bool allowToWrite = false;
        public bool isCalcSequence = false;
        public int calculate()
        {

            if (state == State.firstOp && wasOperatorPressed)
            {
                operand = accumulator;
                isFirstInput = true;
                switch (checkedOp)
                {
                    case CheckedOp.nul: break;
                    case CheckedOp.add: accumulator += operand; break;
                    case CheckedOp.sub: accumulator -= operand; break;
                    case CheckedOp.mul: accumulator *= operand; break;
                    case CheckedOp.div:
                        if (accumulator == 0 && operand == 0)
                            return (int)ERRORS.DivZeroByZero;
                        if (operand == 0)
                            return (int)ERRORS.DivByZero;
                        accumulator /= operand;
                        break;
                    default: break;
                }
            }

            if (!isCalcSequence)
            {
                int status = doOperation(checkedOp);
                if (status != 0) return status;
                isCalcSequence = true;
                //checkedOp = CheckedOp.nul;
            }
            else
            {
                isFirstInput = true;
                accumulator = Double.Parse(numberText);
                switch (checkedOp)
                {
                    case CheckedOp.nul: break;
                    case CheckedOp.add: accumulator += operand; break;
                    case CheckedOp.sub: accumulator -= operand; break;
                    case CheckedOp.mul: accumulator *= operand; break;
                    case CheckedOp.div:
                        if (accumulator == 0 && operand == 0)
                            return (int)ERRORS.DivZeroByZero;
                        if (operand == 0)
                            return (int)ERRORS.DivByZero;
                        accumulator /= operand;
                        break;
                    default: break;
                }
            }

            if (checkedOp == CheckedOp.nul)
            {
                accumulator = Double.Parse(numberText);
            }
            else
                numberText = Convert.ToString(accumulator);

            isFirstEntry = true;
            state = State.firstOp;
            wasOperatorPressed = false;
            isResult = true;
            if (numberText.Length >= 16) allowToWrite = true;
            else 
                allowToWrite = false;

            history.clearEquation();
            return 0;
        }
    }
}
