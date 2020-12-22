using System.Collections.Generic;
using System.Linq;
using ChemSharp.Extensions;

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
        /// <param name="theory"></param>
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

        /// <summary>
        /// gets best composition
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="exp"></param>
        /// <param name="impurities"></param>
        /// <returns></returns>
        public static double[] Solve(string formula, Dictionary<string, double> exp, List<Impurity> impurities)
        {
            var calc = new HashSet<Result>();
            //get all combinations
            var comp = new HashSet<List<double>>();
            foreach (var range in from imp in impurities
                let count = (int) ((imp.Upper - imp.Lower) / imp.Step) + 1 //add 0 to range
                select CollectionsUtil.LinearRange(imp.Lower, imp.Upper, count))
            {
                comp.Add(range.ToList());
            }

            foreach (var vec in comp.Cartesian())
            {
                //build testformula
                var vecArray = vec.ToArray();
                var testFormula = formula.SumFormula(impurities, vecArray);
                calc.Add(new Result(testFormula, Error(testFormula.ElementalAnalysis(),exp), vecArray));
            }
            return calc.OrderBy(s => s.Err).First().Vec;
        }


        /// <summary>
        /// Build Sum Formula with impurities
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="impurities"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static string SumFormula(this string formula, List<Impurity> impurities, double[] vec)
        {
            var testFormula = formula;
            for (var i = 0; i < vec.Count(); i++) testFormula += impurities[i].Formula.CountElements().Factor(vec.ElementAt(i)).Parse();
            return testFormula;
        }
    }
}
