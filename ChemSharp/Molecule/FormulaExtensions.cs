using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChemSharp.Molecule
{
    public static class FormulaExtensions
    {
        private static string Pattern = @"([A-Z][a-z]*)(\d*[,.]?\d*)|(\()|(\))(\d*[,.]?\d*)";

        /// <summary>
        /// Converts List of Elements to Sum Formula
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SumFormula(this IEnumerable<Element> input)
        {
            var groups = from element in input
                         group element by element.Symbol
                into newGroup
                         orderby newGroup.Key
                         select newGroup;
            var formula = "";
            foreach (var group in groups)
            {
                var count = group.Count();
                formula += $"{group.Key}{(count != 1 ? count.ToString() : "")}";
            }

            return formula;
        }

        /// <summary>
        /// Calculate Molecular Weight
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Weight(this IEnumerable<Element> input)
        {
            return input.Sum(s => s.AtomicWeight);
        }

        /// <summary>
        /// Conversion from string to ElementCollection
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<Element> ToElements(this string input)
        {
            //replace complex brackets
            input = input.Replace("[", "(".Replace("]", ")"));
            List<List<Element>> result = new List<List<Element>>()
            {
                new List<Element>()
            };
            var index = 0;
            foreach (Match m in Regex.Matches(input, Pattern))
            {
                if (m.Groups[1].Success)
                    for (var i = 0; i < Multiplicity(m.Groups[2]); i++)
                        result[index].Add(new Element(m.Groups[1].Value));
                if (m.Groups[3].Success)
                {
                    index++;
                    result.Add(new List<Element>());
                }
                if (m.Groups[4].Success)
                    for (var j = index; j < result.Count; j++)
                        result[j].Factor(Multiplicity(m.Groups[5]));
            }
            return result.Combine();
        }

        /// <summary>
        /// Combines a list of lists into single list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<T> Combine<T>(this List<List<T>> input)
        {
            var tmp = new List<T>();
            foreach (var list in input) tmp.AddRange(list);
            return tmp;
        }

        /// <summary>
        /// calculates multiplicity
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static int Multiplicity(Group match) => match.Success && match.Value != ""
            ? Convert.ToInt32(match.Value)
            : 1;

        /// <summary>
        /// Multiplies a List of Element
        /// </summary>
        /// <param name="input"></param>
        /// <param name="multiplicity"></param>
        /// <returns></returns>
        private static List<Element> Factor(this List<Element> input, int multiplicity)
        {
            var tmp = new List<Element>(input);
            for (int i = 0; i < multiplicity - 1; i++) input.AddRange(tmp);
            return input;
        }
    }
}