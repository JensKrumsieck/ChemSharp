using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules
{
    public class Molecule
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
            if (provider is IBondDataProvider bondProvider) BondDataProvider = bondProvider;
            else RecalculateBonds();
        }

        private IAtomDataProvider _atomDataProvider;
        ///<summary>
        /// Provides AtomData
        /// </summary>
        public IAtomDataProvider AtomDataProvider
        {
            get => _atomDataProvider;
            set
            {
                _atomDataProvider = value;
                Atoms = _atomDataProvider.Atoms.ToList();
            }
        }

        private IBondDataProvider _bondDataProvider;
        ///<summary>
        /// Provides BondData
        /// </summary>
        public IBondDataProvider BondDataProvider
        {
            get => _bondDataProvider;
            set
            {
                _bondDataProvider = value;
                Bonds = _bondDataProvider.Bonds.ToList();
            }
        }

        /// <summary>
        /// Wrapper for Centroid Method
        /// </summary>
        public Vector3 Centroid => Atoms.Centroid();

        /// <summary>
        /// Recalculates Bonds based on Distances
        /// WARNING! Discards every existing Bond information
        /// Add a IBondDataProvider Instance to use Bonds read from file
        /// WARNING! Any IBondDataProvider will be discarded by <see cref="RecalculateBonds"/>
        /// </summary>
        public void RecalculateBonds()
        {
            //discard data provider and reset bonds
            _bondDataProvider = null;
            Bonds = new List<Bond>();
            var matched = new HashSet<(int, int)>();
            for (var i = 0; i < Atoms.Count(); i++)
            {
                for (var j = i + 1; j < Atoms.Count(); j++)
                {
                    if (i == j || !Atoms.ElementAt(i).InternalBondTo(Atoms.ElementAt(j)) ||
                        (matched.Contains((i, j)) && matched.Contains((j, i)))) continue;
                    matched.Add((i, j));
                    Bonds.Add(new Bond(Atoms.ElementAt(i), Atoms.ElementAt(j)));
                }
            }
        }

        /// <summary>
        /// Updates coordinate system mapping function
        /// </summary>
        /// <param name="mapping"></param>
        public void SetMapping(Func<Vector3, Vector3> mapping)
        {
            foreach (var atom in Atoms)
                atom.Mapping = mapping;
        }
    }
}
