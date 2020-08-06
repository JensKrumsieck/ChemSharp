using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChemSharp.Molecule
{
    public static class Extensions
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
            input = input.Replace("[", "(".Replace("]", ")"));
            List<List<Element>> result = new List<List<Element>>()
            {
                new List<Element>()
            };
            var index = 0;
            foreach (Match m in Regex.Matches(input, Pattern))
            {
                if (m.Groups[1].Success)
                {
                    var amount = m.Groups[2].Success && m.Groups[2].Value != "" ? Convert.ToInt32(m.Groups[2].Value) : 1;
                    for (var i = 0; i < amount; i++)result[index].Add(new Element(m.Groups[1].Value));
                }
                if (m.Groups[3].Success)
                {
                    index++;
                    result.Add(new List<Element>());
                }
                if (m.Groups[4].Success)
                {
                    var multiplicity = m.Groups[5].Success && m.Groups[5].Value != ""
                        ? Convert.ToInt32(m.Groups[5].Value)
                        : 1;
                    for (var j = index; j < result.Count; j++) result[j].Factor(multiplicity);
                }
            }

            var ret = new List<Element>();
            foreach(var list in result) ret.AddRange(list);
            return ret;
        }

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