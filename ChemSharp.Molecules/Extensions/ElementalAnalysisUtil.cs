using ChemSharp.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemSharp.Molecules.ElementalAnalysis;

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
            var err = new ConcurrentBag<double>();

            Parallel.ForEach(theory, item =>
            {
                var (key, _) = item;
                if (exp.ContainsKey(key)) err.Add(System.Math.Pow(exp[key] - theory[key], 2));
            });
            return System.Math.Sqrt(err.Sum()) * err.Max();
        }

        /// <summary>
        /// gets best composition
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="exp"></param>
        /// <param name="impurities"></param>
        /// <returns></returns>
        public static double[] Solve(string formula, Dictionary<string, double> exp, IEnumerable<Impurity> impurities)
        {
            var calc = new ConcurrentBag<Result>();

            //get all combinations
            var comp = new ConcurrentBag<HashSet<double>>();
            var imps = impurities.ToArray();
            Parallel.ForEach(imps, imp =>
            {
                var count = (int)((imp.Upper - imp.Lower) / imp.Step) + 1; //add 0 to range
                comp.Add(CollectionsUtil.LinearRange(imp.Lower, imp.Upper, count).ToHashSet());
            });
            Parallel.ForEach(comp.Cartesian(), item =>
            {
                var vecArray = item.ToArray();
                var testFormula = formula.SumFormula(imps, vecArray);
                calc.Add(new Result(testFormula, Error(testFormula.ElementalAnalysis(), exp), vecArray));
            });

            return calc.OrderBy(s => s.Err).First().Vec;
        }

        /// <summary>
        /// Build Sum Formula with impurities
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="impurities"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static string SumFormula(this string formula, IEnumerable<Impurity> impurities, IEnumerable<double> vec)
        {
            var testFormula = formula;
            var imps = impurities.ToArray();
            var vecArr = vec.ToArray();
            for (var i = 0; i < vecArr.Count(); i++) testFormula += imps[i].Formula.CountElements().Factor(vecArr[i]).Parse();
            return testFormula;
        }
    }
}
