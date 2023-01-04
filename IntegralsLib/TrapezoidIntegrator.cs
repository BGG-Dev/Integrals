using MathNet.Numerics;

namespace IntegralsLib
{
    public class TrapezoidIntegrator : IIntegrator
    {
        private int _m2CandidateCount;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrapezoidIntegrator()
        {
            _m2CandidateCount = 4096;
        }

        public double Integrate(Func<double, double> f, double a, double b, double epsilon)
        {
            // Calculating M2
            double m2 = CalculateM2(f, a, b);

            // Calculating h
            double h = Math.Sqrt((12.0 * epsilon) / (m2 * Math.Abs(b - a)));

            // Calculating n
            int n = (int)Math.Ceiling(Math.Abs(b - a) / h);

            // Integrating
            double s = 0;
            double[] xs = Generate.LinearSpaced(n + 1, a, b);
            s += f.Invoke(xs[0]);
            s += f.Invoke(xs[n]);
            s /= 2.0;
            for (var i = 1; i < n; i++)
            {
                s += f.Invoke(xs[i]);
            }
            s *= h;

            // Returning
            return s;
        }

        /// <summary>
        /// Method to calculate M2 = max |f''(x)|,
        /// a <= x <= b
        /// </summary>
        /// <param name="f"> Original function </param>
        /// <param name="a"> Integration start </param>
        /// <param name="b"> Integration end </param>
        /// <returns> M2 value </returns>
        private double CalculateM2(Func<double, double> f, 
                                   double a, double b)
        {
            // Returning M2
            return Enumerable.Max(Generate.LinearSpaced(_m2CandidateCount, a, b)
                                          .Select(x => Math.Abs(Differentiate.Derivative(f, x, 2))));
        }
    }
}
