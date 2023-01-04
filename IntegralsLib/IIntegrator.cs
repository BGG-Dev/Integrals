namespace IntegralsLib
{
    /// <summary>
    /// Represents integration contract
    /// </summary>
    public interface IIntegrator
    {
        /// <summary>
        /// Method - integration contract
        /// </summary>
        /// <param name="f"> function to integrate </param>
        /// <param name="a"> min </param>
        /// <param name="b"> max </param>
        /// <param name="epsilon"> precision used to calculate h and n</param>
        /// <returns> numeric integral value </returns>
        public abstract double Integrate(Func<double, double> f, 
                                         double a, double b,
                                         double epsilon);
    }
}
