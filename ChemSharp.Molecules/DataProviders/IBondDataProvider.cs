using System.Collections.Generic;

namespace ChemSharp.Molecules.DataProviders
{
    public interface IBondDataProvider
    {
        public IEnumerable<Bond> Bonds { get; set; }
    }
}
