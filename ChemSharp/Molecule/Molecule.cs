using ChemSharp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecule
{
    public class Molecule
    {
        /// <summary>
        /// Title for Molecule
        /// </summary>
        public string Title { get; set; }

        public IEnumerable<Atom> Atoms { get; set; }

        public IEnumerable<Bond> Bonds { get; set; }

        public Molecule(IEnumerable<Atom> atoms, IEnumerable<Bond> bonds = null)
        {
            Atoms = atoms;
            Bonds = bonds;
            if (bonds == null)
                Bonds = GenerateBonds();
        }

        /// <summary>
        /// Determines if two Atoms are bond together.
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public bool IsBond(Atom a1, Atom a2) => Bonds.FirstOrDefault(s =>
                                                      (s.Atom1.Equals(a1) && s.Atom2.Equals(a2))
                                                      || (s.Atom2.Equals(a1) && s.Atom1.Equals(a2)))
                                                  != null;

        /// <summary>
        /// Wrapper for IEnumerable<Atom>.Centroid()
        /// </summary>
        public Vector3 Centroid => Atoms.Centroid();

        /// <summary>
        /// Generate Bonds if not given
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Bond> GenerateBonds()
        {
            var matched = new HashSet<(int, int)>();
            for (var i = 0; i < Atoms.Count(); i++)
            {
                for (var j = i + 1; j < Atoms.Count(); j++)
                {
                    if (i != j
                        && Atoms.ElementAt(i).BondTo(Atoms.ElementAt(j))
                        && (!matched.Contains((i, j)) || !matched.Contains((j, i))))
                    {
                        matched.Add((i, j));
                        yield return new Bond(Atoms.ElementAt(i), Atoms.ElementAt(j));
                    }
                }
            }
        }
    }
}
