using ChemSharp.Molecule;
using System.Collections.Generic;

namespace ChemSharp.Files.Molecule
{
    interface IAtomFile
    {
        public IEnumerable<Atom> Atoms { get; set; }
    }
}
