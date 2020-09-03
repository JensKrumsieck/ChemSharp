using System.Collections.Generic;
using ChemSharp.Molecule;

namespace ChemSharp.Files.Molecule
{
    public interface IBondFile
    {
        public IEnumerable<Bond> Bonds { get; set; }
    }
}
