using System.Collections.Generic;

namespace ChemSharp.Molecule
{
    public class Molecule
    {
        public IEnumerable<Atom> Atoms { get; set; }

        public Molecule(IEnumerable<Atom> atoms)
        {
            Atoms = atoms;
            //as the atom only constructor is used, generate bond information
            GenerateBonds();
        }

        public void GenerateBonds()
        {
            //TODO!
        }
    }
}
