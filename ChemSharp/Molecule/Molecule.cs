using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecule
{
    public class Molecule
    {
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
        /// Generate Bonds if not given
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Bond> GenerateBonds()
        {
            var matched = new HashSet<(int, int)>();
            for (var i = 0; i < Atoms.Count(); i++)
            {
                for (var j = i+1; j < Atoms.Count(); j++)
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
