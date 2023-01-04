using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;

namespace IWPF.service
{
    static internal class FunctionService
    {
        /// <summary>
        /// Creates Func instance, using given string as function definition
        /// </summary>
        /// <param name="fSrc"> string to use as definition </param>
        /// <returns> Function instance </returns>
        public static Func<double, double> CreateFunctionFromString(string fSrc)
        {
            var str = "x => " + fSrc;
            var options = ScriptOptions.Default.AddImports(new string[] { "System" });
            return CSharpScript.EvaluateAsync<Func<double, double>>(str, options).Result;
        }

        /// <summary>
        /// Evaluates C# expression as double
        /// </summary>
        /// <param name="src"> string to take expression from </param>
        /// <returns> evaluated as double </returns>
        public static double EvaluateAsDouble(string src)
        {
            var options = ScriptOptions.Default.AddImports(new string[] { "System" });
            return CSharpScript.EvaluateAsync<double>(src, options).Result;
        }
    }
}
