using MathNet.Numerics;

namespace IntegralsLib
{
    /// <summary>
    /// Gauss integration implementation
    /// </summary>
    public class GaussIntegrator : IIntegrator
    {
        private class GaussPoint
        {
            public double[] Abscissas { get; }

            public double[] Weights { get; }

            internal int Order { get; }

            private GaussPoint(int order, double[] abscissas, double[] weights)
            {
                Abscissas = abscissas;
                Weights = weights;
                Order = order;
            }

            public static GaussPoint Generate(int order, double eps)
            {
                double w0 = 0; // Weights
                int m = order;
                double[] abscissas = new double[m];
                double[] weights = new double[m];
                double t0 = 1.0 - (1.0 - 1.0 / order) / (8.0 * order * order);
                double t1 = 1.0 / (4.0 * order + 2.0);

                // Find ith root of Legendre polynomial
                for (int i = 1; i <= m; i++)
                {
                    int j = 0;
                    double x0 = Math.Cos(Math.PI * ((i << 2) - 1) * t1) * t0; // Initial guess
                    double x1, dx; // Abscissas
                    double w1, dw; // Weights

                    // Newton iterations, at least one
                    do
                    {
                        // Legendre polynomial values
                        double p1 = 1.0;
                        double p0 = x0;
                        double p2;
                        int k;
                        double t2;

                        for (k = 2; k <= order; k++)
                        {
                            p2 = p1;
                            p1 = p0;
                            t2 = x0 * p1;
                            double t3 = (k - 1.0) / k;

                            p0 = t2 + t3 * (t2 - p2);
                        }

                        double dpdx = ((x0 * p0 - p1) * order) / (x0 * x0 - 1.0); // Compute Legendre polynomial derivative at x0

                        x1 = x0 - p0 / dpdx; // Newton step

                        w1 = 2.0 / ((1.0 - x1 * x1) * dpdx * dpdx); // Weight computing

                        // Compute weight w0 on first iteration, needed for dw
                        if (j == 0)
                        {
                            w0 = 2.0 / ((1.0 - x0 * x0) * dpdx * dpdx);
                        }

                        dx = x0 - x1;
                        dw = w0 - w1;

                        x0 = x1;
                        w0 = w1;
                        j++;
                    }
                    while ((Math.Abs(dx) > eps || Math.Abs(dw) > eps) && j < 100);

                    int index = (m - 1) - (i - 1);

                    abscissas[index] = x1;
                    weights[index] = w1;
                }

                return new GaussPoint(order, abscissas, weights);
            }
        }

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
            // Going from a b to -1 1
            var g = (double t) => f((b - a) / 4.0 * t * (3.0 - t * t) + (b + a) / 2.0) * 3.0 * (b - a) / 4.0 * (1.0 - t * t);

            // Returning
            return Integrate11(g, 64, epsilon);
        }

        /// <summary>
        /// Runs Gauss integration for [-1; 1]
        /// </summary>
        /// <param name="f"> function to integrate on [-1; 1] </param>
        /// <param name="epsilon"> precision </param>
        /// <returns> numeric integral value </returns>
        private double Integrate11(Func<double, double> f, int order, double epsilon)
        {
            GaussPoint gaussLegendrePoint = GaussPoint.Generate(order, epsilon);

            double sum, ax;
            int i;
            int m = (order + 1) >> 1;

            double a = 0.5 * (1 - (-1));
            double b = 0.5 * (1 + (-1));

            var weights = gaussLegendrePoint.Weights;
            var abscissas = gaussLegendrePoint.Abscissas;

            if (order.IsOdd())
            {
                sum = weights[0]*f(b);
                for (i = 1; i < m; i++)
                {
                    ax = a*abscissas[i];
                    sum += weights[i]*(f(b + ax) + f(b - ax));
                }
            }
            else
            {
                sum = 0.0;
                for (i = 0; i < m; i++)
                {
                    ax = a*abscissas[i];
                    sum += weights[i]*(f(b + ax) + f(b - ax));
                }
            }

            return a*sum;
        }
    }
}
