using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemSharp.Molecule
{
    public static class Extensions
    {

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
    }
}
