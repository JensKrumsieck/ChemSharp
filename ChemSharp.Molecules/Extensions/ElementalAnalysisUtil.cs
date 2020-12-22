using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecules.Extensions
{
    public static class ElementalAnalysisUtil
    {
        /// <summary>
        /// calculates the CHN Analysis
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static Dictionary<string, double> MassComposition(string formula)
        {
            var elements = new Dictionary<string, double>();
            var counted = formula.CountElements();
            foreach (var (element, amount) in counted) elements.Add(element, SumFormulaUtil.FragmentWeight(element, amount));
            return elements;
        }

        /// <summary>
        /// Calculates CHN Analysis in percent
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static Dictionary<string, double> ElementalAnalysis(this string formula) => MassComposition(formula)
            .ToDictionary(item => item.Key, item => System.Math.Round(item.Value / formula.MolecularWeight() * 100, 3));

        /// <summary>
        /// Calculates Deviation of Exp and Theoretical EA
        /// </summary>
        /// <param name="theory"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static Dictionary<string, double> Deviation(Dictionary<string, double> theory, Dictionary<string, double> exp) =>
            theory.Where(item => exp.ContainsKey(item.Key) && exp[item.Key] != 0d)
                .ToDictionary(item => item.Key,
                item => System.Math.Round(System.Math.Abs(item.Value - exp[item.Key]), 3));

        /// <summary>
        /// calculates error between two analysis
        /// </summary>
        /// <param name="theo"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double Error(Dictionary<string, double> theory, Dictionary<string, double> exp)
        {
            var err = new HashSet<double>();
            foreach (var (key, _) in theory)
            {
                if (!exp.ContainsKey(key)) continue;
                err.Add(System.Math.Pow(exp[key] - theory[key], 2));
            }
            return System.Math.Sqrt(err.Sum()) * err.Max();
        }
    }
}
