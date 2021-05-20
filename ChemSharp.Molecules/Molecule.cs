using ChemSharp.DataProviders;
using ChemSharp.Export;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Molecules.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecules
{
    public class Molecule : IExportable
    {
        /// <summary>
        /// Title for Molecule
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Molecules Atoms
        /// </summary>
        public List<Atom> Atoms { get; set; }

        /// <summary>
        /// The Molecules Bonds
        /// </summary>
        public List<Bond> Bonds { get; set; }

        /// <summary>
        /// creates Molecule without Atoms to add later
        /// </summary>
        public Molecule() { }
        /// <summary>
        /// creates Molecule with IEnumerable of Atoms
        /// </summary>
        /// <param name="atoms"></param>
        /// <param name="bonds"></param>
        public Molecule(IEnumerable<Atom> atoms, IEnumerable<Bond> bonds = null) : this()
        {
            Atoms = atoms.ToList();
            if (bonds != null) Bonds = bonds.ToList();
            else RecalculateBonds();
        }

        /// <summary>
        /// creates Molecule with provider
        /// </summary>
        /// <param name="provider"></param>
        public Molecule(IAtomDataProvider provider) : this()
        {
            AtomDataProvider = provider;
            Atoms = provider.Atoms.ToList();
            if (provider is IBondDataProvider { Bonds: { } } bondProvider && bondProvider.Bonds.Any())
            {
                BondDataProvider = bondProvider;
                Bonds = bondProvider.Bonds.ToList();
            }
            else RecalculateBonds();
            Title = ((AbstractDataProvider)AtomDataProvider).Path;
        }

        ///<summary>
        /// Provides AtomData
        /// </summary>
        public readonly IAtomDataProvider AtomDataProvider;

        ///<summary>
        /// Provides BondData
        /// </summary>
        public IBondDataProvider BondDataProvider { get; private set; }

        /// <summary>
        /// Recalculates Bonds based on Distances
        /// WARNING! Discards every existing Bond information
        /// Add a IBondDataProvider Instance to use Bonds read from file
        /// WARNING! Any IBondDataProvider will be discarded by <see cref="RecalculateBonds"/>
        /// </summary>
        public void RecalculateBonds()
        {
            //discard data provider and reset bonds
            BondDataProvider = null;
            Bonds = new List<Bond>();
            var matched = new HashSet<(int, int)>();
            for (var i = 0; i < Atoms.Count; i++)
            {
                for (var j = i + 1; j < Atoms.Count; j++)
                {
                    if (i == j || !Atoms[i].BondToByCovalentRadii(Atoms[j]) ||
                        (matched.Contains((i, j)) && matched.Contains((j, i)))) continue;
                    matched.Add((i, j));
                    Bonds.Add(new Bond(Atoms[i], Atoms[j]));
                }
            }
        }

        /// <summary>
        /// returns neighbors of specific atom
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public IEnumerable<Atom> Neighbors(Atom a) => AtomUtil.Neighbors(a, this);

        /// <summary>
        /// returns nonmetal neighbors of specific atom
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public IEnumerable<Atom> NonMetalNeighbors(Atom a) => Neighbors(a) != null && Neighbors(a).Any() ? Neighbors(a).Where(s => s.IsNonMetal) : Enumerable.Empty<Atom>();
    }
}
