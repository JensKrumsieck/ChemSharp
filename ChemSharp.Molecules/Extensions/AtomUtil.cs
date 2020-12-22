using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecules.Extensions
{
    public static class AtomUtil
    {
        /// <summary>
        /// Calculates MolecularWeight of given Atoms collection
        /// </summary>
        /// <param name="atoms"></param>
        /// <returns></returns>
        public static double MolecularWeight(this IEnumerable<Atom> atoms) => atoms.Sum(s => s.AtomicWeight);

        /// <summary>
        /// Creates Sum Formula from given atoms collection
        /// </summary>
        /// <param name="atoms"></param>
        /// <returns></returns>
        public static string SumFormula(this IEnumerable<Atom> atoms)
        {
            var groups = atoms.Group();
            var formula = "";
            foreach (var g in groups)
            {
                var count = g.Count();
                formula += $"{g.Key}{(count != 1 ? count.ToString() : "")}";
            }
            return formula;
        }

        /// <summary>
        /// Returns Elements grouped by Symbol
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static IEnumerable<IGrouping<string, Element>> Group(this IEnumerable<Element> input)
            => from element in input
               group element by element.Symbol
                into newGroup
               orderby newGroup.Key
               select newGroup;
    }
}