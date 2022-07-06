using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            calc.resetNumberText();
            tbEquations.Text = calc.numberText;
            initVars();
        }

        static Calculations calc = new Calculations();

        public int lengthMax = 0;
        public string tbEquationsPlaceholder = "0";
        public string textForOutput = "0";
        string numFormat = "G13";
        bool isFreezed = false;
        public struct FontSizes
        {
            public FontSizes() { }
            public double lenMoreThan16 { get; private set; } = 15.5;
            public double lenMoreThan12 { get; private set; } = 18.5;
            public double byDefault { get; private set; } = 23;
        }
        FontSizes fontSizes = new FontSizes();

        public struct NumberLengths
        {
            public NumberLengths() { }
            public int byDefault { get; private set; } = 16;
            public int extended { get; private set; } = 17;
            public int toSmall { get; private set; } = 12;
            public int toSmaller { get; private set; } = 16;
        }//tbEquationsHistory.Text = string.Empty;
        NumberLengths numberLengths = new NumberLengths();

        private void initVars() //without this thing it does not want to assign a value from the struct (IDE1007)
        {
            lengthMax = numberLengths.byDefault;
        }
        //lengthMax = numberLengths.byDefault; //?????????????????????????????????????????????????????????

        private void format()
        {
            if (tbEquations.Text.Length > numberLengths.toSmaller) tbEquations.FontSize = fontSizes.lenMoreThan16;
            else if (tbEquations.Text.Length > numberLengths.toSmall) tbEquations.FontSize = fontSizes.lenMoreThan12;
            else tbEquations.FontSize = fontSizes.byDefault;
        }

        private void btnDigit_Click(object sender, RoutedEventArgs e) // and '.'
        {
            if (!isFreezed)
            {
                char gotChar = (sender as Button).Content.ToString()[0]; //get char from content of a button
                if (gotChar == '.' && calc.numberText.Contains('.')) return;
                //if (Convert.ToString(calc.accumulator).Length > lengthMax)
                //if (calc.state == Calculations.State.nextOp)
                //{
                //        tbEquations.Text = tbEquationsPlaceholder;
                //} else
                if (calc.numberText.Length >= lengthMax && calc.allowToWrite == true) lengthMax = calc.numberText.Length + 1;
                else
                    lengthMax = numberLengths.byDefault;
                if (calc.numberText.Length < lengthMax || (gotChar == '.' && !calc.numberText.Contains('.')))
                {
                    //if (gotChar == '.' && !calc.numberText.Contains('.') && !calc.isFirstInput)
                    //    lengthMax = numberLengths.extended;

                    calc.pushToNumber(gotChar);
                    tbEquations.Text = calc.numberText;

                }
                else SystemSounds.Beep.Play();
            }
            else SystemSounds.Beep.Play();
            format();
        }
        private void btnClearInput_Click(object sender, RoutedEventArgs e)
        {
            if (isFreezed)
            {
                calc.resetEquation();
                tbEquations.Text = calc.numberText;
                tbEquationsHistory.Text = string.Empty;
                isFreezed = false;
            }
            else
            {
                calc.resetNumberText();
                tbEquations.Text = calc.numberText;
            }
            format();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            calc.resetEquation();
            tbEquations.Text = calc.numberText;
            tbEquationsHistory.Text = string.Empty;

            isFreezed = false;

            format();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.add();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                {
                    tbEquations.Text = calc.accumulator.ToString(numFormat);
                    tbEquationsHistory.Text = calc.history.getCraftedEquation();
                }
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.subtract();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                {
                    tbEquations.Text = calc.accumulator.ToString(numFormat);
                    tbEquationsHistory.Text = calc.history.getCraftedEquation();
                }
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.multiply();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                {
                    tbEquations.Text = calc.accumulator.ToString(numFormat);
                    tbEquationsHistory.Text = calc.history.getCraftedEquation();
                }
            }
            else SystemSounds.Beep.Play();
            format();
        }

        string errorDescription(ERRORS err)
        {
            isFreezed = true;
            switch (err) {
                case ERRORS.DivZeroByZero:     return "Result is undefined";
                case ERRORS.DivByZero:         return "Cannot divide by zero";
                case ERRORS.Overflow:          return "Overflow";
                case ERRORS.SqrtOfNeg:         return "Invalid input";
                case ERRORS.InvalidInput:      return "Truly invalid input";
            }
            return "";
        }

        private void btnDivide_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.divide();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                {
                    tbEquations.Text = calc.accumulator.ToString(numFormat);
                    tbEquationsHistory.Text = calc.history.getCraftedEquation();
                }
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnСalculate_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.calculate();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                {
                    tbEquations.Text = calc.accumulator.ToString(numFormat);
                    tbEquationsHistory.Text = calc.history.getCraftedEquation();
                }
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnSignInverse_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.signInverse();
                tbEquations.Text = calc.numberText;
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.backspace();
                if (!calc.isResult) tbEquations.Text = calc.numberText;
                else SystemSounds.Beep.Play();
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnPercent_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.percent();
                tbEquations.Text = calc.numberText;
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnSqrt_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.sqrt();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                    tbEquations.Text = calc.numberText;
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnReciproc_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                ERRORS status = (ERRORS)calc.reciproc();
                if (status != 0)
                {
                    calc.history.clearEquation();
                    //tbEquationsHistory.Text = string.Empty;
                    tbEquations.Text = errorDescription(status);
                    SystemSounds.Beep.Play();
                    isFreezed = true;
                }
                else
                    tbEquations.Text = calc.numberText;
            }
            else SystemSounds.Beep.Play();
            format();
        }

        private void btnMClear_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.clearMemory();
                tbMemorySign.Text = string.Empty;
            }
            else SystemSounds.Beep.Play();
        }

        private void btnMReveal_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.extractMemory();
                tbEquations.Text = calc.numberText;
                format();
            }
            else SystemSounds.Beep.Play();
        }

        private void btnMSave_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.setMemory();
                tbMemorySign.Text = "M";
            }
            else SystemSounds.Beep.Play();
        }

        private void btnMAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.addToMemory();
                tbMemorySign.Text = "M";
            }
            else SystemSounds.Beep.Play();
        }

        private void btnMSub_Click(object sender, RoutedEventArgs e)
        {
            if (!isFreezed)
            {
                calc.subFromMemory();
                tbMemorySign.Text = "M";
            }
            else SystemSounds.Beep.Play();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            if (shift == true && e.Key == Key.D8)
            {
                btnMultiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }

            if (shift == true && e.Key == Key.D5)
            {
                btnPercent.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }

            switch (e.Key)
            {
                case Key.Back:
                    btnBackspace.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D1:
                    btnDigit1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D2:
                    btnDigit2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D3:
                    btnDigit3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D4:
                    btnDigit4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D5:
                    btnDigit5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D6:
                    btnDigit6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D7:
                    btnDigit7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D8:
                    btnDigit8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D9:
                    btnDigit9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.D0:
                    btnDigit0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.Divide:
                    btnDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.OemPlus:
                    btnPlus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.OemMinus:
                    btnMinus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.OemQuestion:
                    btnDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.Add:
                    btnPlus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.Subtract:
                    btnMinus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.Multiply:
                    btnMultiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.Decimal:
                    btnDot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                case Key.OemPeriod:
                    btnDot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
            }
        }

    }
}
