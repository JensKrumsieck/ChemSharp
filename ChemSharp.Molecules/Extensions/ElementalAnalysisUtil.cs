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
    }
}
