using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecules.Extensions
{
    public static class AtomExtensions
    {
        /// <summary>
        /// Calculates MolecularWeight of given Atoms collection
        /// </summary>
        /// <param name="atoms"></param>
        /// <returns></returns>
        public static double MolecularWeight(this IEnumerable<Atom> atoms) => atoms.Sum(s => s.AtomicWeight);
    }
}
