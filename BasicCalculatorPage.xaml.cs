using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppPerfect
{
    public sealed partial class BasicCalculatorPage : Page
    {
        double currentValue = 0;
        string currentOperator = "";
        bool isNewEntry = true;
        double lastEnteredValue = 0;

        public BasicCalculatorPage()
        {
            this.InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
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

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
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
            lastEnteredValue = num;
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
