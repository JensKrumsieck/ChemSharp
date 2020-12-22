using System.Collections.Generic;

namespace ChemSharp.Molecules.DataProviders
{
    public interface IAtomDataProvider
    {
        public IEnumerable<Atom> Atoms { get; set; }
    }
}
