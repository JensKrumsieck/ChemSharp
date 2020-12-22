using ChemSharp.Molecules.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChemSharp.Molecules.Extensions
{
    public static class SumFormulaUtil
    {
        /// <summary>
        /// the regex pattern for molecular formulas
        /// </summary>
        public const string Pattern = @"([A-Z][a-z]*)(\d*[,.]?\d*)|(\()|(\))(\d*[,.]?\d*)";

        /// <summary>
        /// Dictionary with Molecular Abbreviations
        /// </summary>
        internal static Dictionary<string, string> Abbreviations = new Dictionary<string, string>
        {
            {"Mes", "C9H11"},//Mesityl
            {"Me", "CH2"}, //Methyl
            {"Et", "C2H5"}, //Ethyl
            {"iPr","C3H6"}, //iso-Propyl
            {"Ph", "C6H5"}, //Phenyl
            {"Ar", "C6H5"}, //Aryl being the same
            {"Bu", "C4H9"}, //Butyl
            {"acac", "C5H7O2"}, //Acetylacetonate (deprotonated)
            {"Bn", "C6H5CH2"}, //Benzyl
            {"Bz", "C6H5CO"}, //benzoyl
            {"Cp", "C5H5"}, //cyclopentadienyl
            {"Cy", "C6H11"}, //cyclohexyl
            {"Fmoc", "C15H11O2"},
            {"Boc", "C5H9O2"}
        };

        /// <summary>
        /// Removes Abbreviations from Formula
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string RemoveAbbreviations(this string formula) =>
            Abbreviations.Aggregate(formula, (current, abbr) => current.Replace(abbr.Key, abbr.Value));

        /// <summary>
        /// counts each element from a given string
        /// (Sum formula with Abbreviations)
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static Dictionary<string, double> CountElements(this string formula)
        {
            formula = formula.RemoveAbbreviations();
            var result = new List<Dictionary<string, double>> { new Dictionary<string, double>() };
            var i = 0;
            foreach (Match m in Regex.Matches(formula, Pattern))
            {
                //Group one == Element string, Assign Group two == Factor
                if (m.Groups[1].Success)
                {
                    //does not contain key -> create
                    if (!result[i].ContainsKey(m.Groups[1].Value)) result[i][m.Groups[1].Value] = (m.Groups[2].Success && m.Groups[2].Value != "" ? Convert.ToDouble(m.Groups[2].Value) : 1d);
                    //contains key -> additon
                    else result[i][m.Groups[1].Value] += (m.Groups[2].Success && m.Groups[2].Value != "" ? Convert.ToDouble(m.Groups[2].Value) : 1d);
                }
                //group 3 == left parentheses
                if (m.Groups[3].Success)
                {
                    i++;
                    result.Add(new Dictionary<string, double>());
                }
                //group 4 == right parentheses; group 5 == multiplicator
                if (!m.Groups[4].Success) continue;
                //add multiplicator to each group element
                var mult = 1d;
                if (m.Groups[5].Success && m.Groups[5].Value != "") mult = Convert.ToDouble(m.Groups[5].Value);
                for (int j = i; j < result.Count; j++) result[j].Factor(mult);
                ////end of subformula
                //i--;
            }
            return result.Merge();
        }

        /// <summary>
        /// Parses empirical Formula as string
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string Parse(this string formula) => CountElements(formula).Parse();

        /// <summary>
        /// Parses dictionary as string
        /// </summary>
        /// <param name="composition"></param>
        /// <returns></returns>
        public static string Parse(this Dictionary<string, double> composition) =>
            string.Join("", (composition.Select(s => s.Key + s.Value)));

        /// <summary>
        /// multiplies each element with factor
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="multiplier">The factor</param>
        /// <returns></returns>
        private static Dictionary<string, double> Factor(this Dictionary<string, double> dict, double multiplier)
        {
            for (var j = 0; j < dict.Count; j++) dict[dict.ElementAt(j).Key] = dict.ElementAt(j).Value * multiplier;
            return dict;
        }

        /// <summary>
        /// merges all dictionaries
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Dictionary<string, double> Merge(this IEnumerable<Dictionary<string, double>> input)
        {
            var dic = new Dictionary<string, double>();
            foreach (var elements in input)
            {
                foreach (var (element, value) in elements)
                {
                    if (dic.ContainsKey(element)) dic[element] += value;
                    else dic.Add(element, value);
                }
            }
            return dic;
        }

        /// <summary>
        /// Gets Weight of Fragment
        /// </summary>
        /// <param name="element"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        internal static double FragmentWeight(string element, double amount)
        {
            var weight = ElementDataProvider.ElementData.FirstOrDefault(s => s.Symbol == element)?.AtomicWeight;
            return weight * amount ?? 0;
        }

        /// <summary>
        /// gets molecular weight
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static double MolecularWeight(this Dictionary<string, double> elements) =>
            elements.Sum(element => FragmentWeight(element.Key, element.Value));

        /// <summary>
        /// Gets Molecular Weight from string
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static double MolecularWeight(this string formula) => MolecularWeight(formula.CountElements());
    }
}
