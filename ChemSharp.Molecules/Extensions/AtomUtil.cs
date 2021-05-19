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
            var groups = atoms.GroupBy(s => s.Symbol).OrderBy(s => s.Key);
            var formula = "";
            foreach (var g in groups)
            {
                var count = g.Count();
                formula += $"{g.Key}{(count != 1 ? count.ToString() : "")}";
            }
            return formula;
        }

        /// <summary>
        /// Gets Saturation of Atom a in Context of Molecule mol
        /// </summary>
        /// <param name="mol"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Saturation(this Molecule mol, Atom a) => Saturation(mol.Bonds, a);

        /// <summary>
        /// Gets Saturation of Atom in Context of Bond List
        /// </summary>
        /// <param name="bonds"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Saturation(this IEnumerable<Bond> bonds, Atom a)
        {
            var selectedBonds = bonds.Bonds(a);
            return selectedBonds.Sum(s => s.Order);
        }

        /// <summary>
        /// Get all Bonds for Atom a in Molecule
        /// </summary>
        /// <param name="mol"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static IEnumerable<Bond> Bonds(this Molecule mol, Atom a) => Bonds(mol.Bonds, a);

        /// <summary>
        /// Get all Bonds containing specific atom
        /// </summary>
        /// <param name="bonds"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static IEnumerable<Bond> Bonds(this IEnumerable<Bond> bonds, Atom a) => bonds.Where(s => s.Atom1.Equals(a) || s.Atom2.Equals(a));

        /// <summary>
        /// Returns Neighboring atoms in context of molecule
        /// </summary>
        /// <param name="a"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static IEnumerable<Atom> Neighbors(Atom a, Molecule ctx) => Neighbors(a, ctx.Bonds);

        /// <summary>
        /// Returns Neighboring atoms in context of bond list
        /// </summary>
        /// <param name="a"></param>
        /// <param name="bonds"></param>
        /// <returns></returns>
        public static IEnumerable<Atom> Neighbors(Atom a, IEnumerable<Bond> bonds) =>
            bonds.Where(s => s.Atoms.Contains(a)).Select(s => s.Atoms.FirstOrDefault(s => !s.Equals(a)));
    }
}