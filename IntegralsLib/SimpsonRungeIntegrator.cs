using MathNet.Numerics;

namespace IntegralsLib
{
    /// <summary>
    /// Simpson-Runge integration implementation
    /// </summary>
    public class SimpsonRungeIntegrator : IIntegrator
    {
        private const double q = 4.0;

        /// <summary>
        /// Method - integration contract implementation
        /// </summary>
        /// <param name="f"> function to integrate </param>
        /// <param name="a"> min </param>
        /// <param name="b"> max </param>
        /// <param name="epsilon"> precision used to calculate h and n</param>
        /// <returns> numeric integral value </returns>
        public double Integrate(Func<double, double> f, double a, double b, double epsilon)
        {
            // Initial segment count
            int n = 1;

            // Integrals for h1 and h2
            double i1 = 0.0, i2 = 0.0;

            // Simpson cycle
            do
            {
                // Multiplying n
                n *= 2;

                // Calculating i1
                i1 = SimpsonFormula(f, a, b, n);

                // Calculating i2
                i2 = SimpsonFormula(f, a, b, n * 2);
            } 
            while (!RungeRule(i1, i2, epsilon));

            // Returning
            return i2;
        }

        /// <summary>
        /// Simpson formula implementation:
        /// i = (h/3) * (y0 + 4s1 + 2s2 + yn)
        /// </summary>
        /// <param name="f"> original function </param>
        /// <param name="a"> integration min </param>
        /// <param name="b"> integration max </param>
        /// <param name="n"> segment count </param>
        /// <returns></returns>
        private double SimpsonFormula(Func<double, double> f, double a, double b, int n)
        {
            // Calculating h
            double h = (b - a) / (double)n;

            // Calculating function values
            double[] ys = Generate.LinearSpaced(n + 1, a, b).Select(x => f.Invoke(x)).ToArray();

            // Calculating s1
            double s1 = 0.0;
            for (int i = 1; i <= n - 1; i += 2)
            {
                s1 += ys[i];
            }

            // Calculating s2
            double s2 = 0.0;
            for (int i = 2; i <= n - 2; i += 2)
            {
                s2 += ys[i];
            }

            // Returning
            return h * (ys[0] + 4.0 * s1 + 2.0 * s2 + ys[n]) / 3.0;
        }

        /// <summary>
        /// Runge rule: |i1 - i2| / (2**q - 1) <= e
        /// </summary>
        /// <param name="i1"> integral, calculated with h1 </param>
        /// <param name="i2"> integral, calculated with h2 = h1/2 </param>
        /// <param name="epsilon"> needed precision </param>
        private bool RungeRule(double i1, double i2, double epsilon)
        {
            return (Math.Abs(i1 - i2)) / (Math.Pow(2.0, q) - 1) <= epsilon;
        }
    }
}
