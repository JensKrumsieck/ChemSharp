using ChemSharp.Molecule;
using System.Collections.Generic;

namespace ChemSharp.Files.Molecule
{
    public interface IBondFile
    {
        public IEnumerable<Bond> Bonds { get; set; }
    }
}
