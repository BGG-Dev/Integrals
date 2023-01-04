using IntegralsLib;
using IWPF.service;
using System;
using System.Windows;

namespace IWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void IntegrationButton_Click(object sender, RoutedEventArgs e)
        {
            // Getting f, a, b
            Func<double, double> f;
            double a = 0;
            double b = 0;
            try
            {
                a = FunctionService.EvaluateAsDouble(MinTextBox.Text);
                b = FunctionService.EvaluateAsDouble(MaxTextBox.Text);
                f = FunctionService.CreateFunctionFromString(FunctionTextBox.Text);
            }
            catch 
            {
                ShowErrorMessageBox("Something went wrong during evaluating function, min, max. Please check input.");
                return;
            }

            // Checking b > a
            if (a >= b)
            {
                ShowErrorMessageBox("max should be BIGGER than min.");
                return;
            }

            // Calculating epsilon
            double epsilon = Math.Pow(10.0, -1.0 * Convert.ToDouble(EpsilonLongUpDown.Value));

            // Integrating as trapezoid
            IIntegrator integrator = new TrapezoidIntegrator();
            double trapezoidResult = integrator.Integrate(f, a, b,epsilon);

            // Integrating as Simpson-Runge
            integrator = new SimpsonRungeIntegrator();
            double srResult = integrator.Integrate(f, a, b,epsilon);

            Result.Text = "Trapezoid:\n";
            Result.Text += "a: " + a.ToString() + " ; b: " + b.ToString() + " ; e: " + epsilon;
            Result.Text += "\n";
            Result.Text += "Result: " + trapezoidResult.ToString();
            Result.Text += "\n\n";
            Result.Text += "Simpson-Runge: " + srResult.ToString();
        }

        private void ShowErrorMessageBox(String msg)
        {
            MessageBox.Show(msg, "Oops!");
        }
    }
}
