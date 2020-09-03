using System.Collections.Generic;
using ChemSharp.Molecule;

namespace ChemSharp.Files.Molecule
{
    interface IBondFile
    {
        public IEnumerable<Bond> Bonds { get; set; }
    }
}
