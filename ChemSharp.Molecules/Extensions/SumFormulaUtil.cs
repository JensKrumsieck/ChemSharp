﻿using ChemSharp.Molecules.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if NETSTANDARD2_0 
using ChemSharp.Extensions;
#endif

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
        internal static Dictionary<string, string> Abbreviations = new()
        {
            { "Mes", "C9H11" },//Mesityl
            { "Me", "CH2" }, //Methyl
            { "Et", "C2H5" }, //Ethyl
            { "iPr", "C3H6" }, //iso-Propyl
            { "Ph", "C6H5" }, //Phenyl
            { "Ar", "C6H5" }, //Aryl being the same
            { "Bu", "C4H9" }, //Butyl
            { "acac", "C5H7O2" }, //Acetylacetonate (deprotonated)
            { "Bn", "C6H5CH2" }, //Benzyl
            { "Bz", "C6H5CO" }, //benzoyl
            { "Cp", "C5H5" }, //cyclopentadienyl
            { "Cy", "C6H11" }, //cyclohexyl
            { "Fmoc", "C15H11O2" },
            { "Boc", "C5H9O2" }
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
            formula = formula.RemoveAbbreviations();//remove special abbreviations
            var parse = Regex.Matches(formula, Pattern);
            var stack = new Stack<Dictionary<string, double>>();
            var tmp = new Dictionary<string, double>();
            stack.Push(tmp);
            foreach (Match m in parse)
            {
                if (m.Groups[1].Success) //Element Name!
                {
                    var top = stack.Peek();
                    if (!top.ContainsKey(m.Groups[1].Value))top[m.Groups[1].Value] = (m.Groups[2].Success && m.Groups[2].Value != "" ? Convert.ToDouble(m.Groups[2].Value) : 1d);
                    //contains key -> additon
                    else top[m.Groups[1].Value] += (m.Groups[2].Success && m.Groups[2].Value != "" ? Convert.ToDouble(m.Groups[2].Value) : 1d);
                }

                if (m.Groups[3].Success) stack.Push(new Dictionary<string, double>()); //left bracket --> new element
                if (m.Groups[4].Success) //right bracket
                {
                    //remove top element and multiply with factor. Merge into new top element
                    var top = stack.Pop();
                    var newTop = stack.Peek();
                    foreach (var (key, value) in top)
                    {
                        var val = value * (m.Groups[5].Success && m.Groups[5].Value != ""
                            ? Convert.ToDouble(m.Groups[5].Value)
                            : 1d);
                        if (!newTop.ContainsKey(key)) newTop.Add(key, val);
                        else
                            newTop[key] += val;
                    }
                }
            }

            return stack.Pop();
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
        internal static Dictionary<string, double> Factor(this Dictionary<string, double> dict, double multiplier)
        {
            for (var j = 0; j < dict.Count; j++) dict[dict.ElementAt(j).Key] = dict.ElementAt(j).Value * multiplier;
            return dict;
        }

        /// <summary>
        /// merges all dictionaries
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static Dictionary<string, double> Merge(this IEnumerable<Dictionary<string, double>> input)
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
            return weight * amount ?? 0d;
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
