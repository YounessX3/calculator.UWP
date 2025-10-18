using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppPerfect
{
    public sealed partial class ScientificCalculatorPage : Page
    {
        double currentValue = 0;
        string currentOperator = "";
        bool isNewEntry = true;

        public ScientificCalculatorPage()
        {
            this.InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string number = btn.Content.ToString();
                if (isNewEntry || Display.Text == "0")
                {
                    Display.Text = number;
                    isNewEntry = false;
                }
                else
                {
                    Display.Text += number;
                }
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string op = btn.Content.ToString();

                if (!double.TryParse(Display.Text, out double num)) return;

                if (currentOperator != "")
                {
                    currentValue = Calculate(currentValue, num, currentOperator);
                    Display.Text = currentValue.ToString();
                }
                else
                {
                    currentValue = num;
                }

                currentOperator = op;
                isNewEntry = true;
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(Display.Text, out double num)) return;

            double result = num;
            if (currentOperator != "")
            {
                result = Calculate(currentValue, num, currentOperator);
                Display.Text = result.ToString();

                if (Window.Current.Content is Frame rootFrame &&
                    rootFrame.Content is MainPage mainPage)
                {
                    mainPage.AddToHistory($"{currentValue} {currentOperator} {num} = {result}");
                }
            }

            currentValue = result;
            currentOperator = "";
            isNewEntry = true;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            currentValue = 0;
            currentOperator = "";
            isNewEntry = true;
        }

        private void Trig_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && double.TryParse(Display.Text, out double value))
            {
                double result = 0;
                switch (btn.Content.ToString())
                {
                    case "sin": result = Math.Sin(value); break;
                    case "cos": result = Math.Cos(value); break;
                    case "tan": result = Math.Tan(value); break;
                }
                Display.Text = result.ToString();
                isNewEntry = true;
            }
        }

        private void Func_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && double.TryParse(Display.Text, out double value))
            {
                double result = 0;
                switch (btn.Content.ToString())
                {
                    case "√": result = Math.Sqrt(value); break;
                    case "x²": result = Math.Pow(value, 2); break;
                    case "π": result = Math.PI; break;
                }
                Display.Text = result.ToString();
                isNewEntry = true;
            }
        }

        private double Calculate(double a, double b, string op)
        {
            switch (op)
            {
                case "+": return a + b;
                case "−": return a - b;
                case "×": return a * b;
                case "÷": return b != 0 ? a / b : 0;
                default: return b;
            }
        }
    }
}
