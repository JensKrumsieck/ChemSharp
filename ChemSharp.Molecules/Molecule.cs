﻿using ChemSharp.DataProviders;
using ChemSharp.Export;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Molecules.Mathematics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public bool CacheNeighborList = true;

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
            var matched = Atoms.Count > 500 ? RecalculateBondsParallel() : RecalculateBondsNonParallel();
            foreach (var (i, j) in matched) Bonds.Add(new Bond(Atoms[i], Atoms[j]));
        }
        private IEnumerable<(int, int)> RecalculateBondsParallel()
        {
            var matched = new ConcurrentStack<(int, int)>();
            Parallel.For(0, Atoms.Count, i =>
            {
                var atomI = Atoms[i];
                for (var j = i + 1; j < Atoms.Count; ++j)
                {
                    var atomJ = Atoms[j];
                    if (i == j || !atomI.BondToByCovalentRadii(atomJ) ||
                        (matched.Contains((i, j)) && matched.Contains((j, i)))) continue;
                    matched.Push((i, j));
                }
            });
            return matched;
        }
        private IEnumerable<(int, int)> RecalculateBondsNonParallel()
        {
            var matched = new Stack<(int, int)>();
            for (var i = 0; i < Atoms.Count; i++)
            {
                var atomI = Atoms[i];
                for (var j = i + 1; j < Atoms.Count; ++j)
                {
                    var atomJ = Atoms[j];
                    if (i == j || !atomI.BondToByCovalentRadii(atomJ) ||
                        (matched.Contains((i, j)) && matched.Contains((j, i)))) continue;
                    matched.Push((i, j));
                }
            }
            return matched;
        }

        private Dictionary<Atom, List<Atom>> _cachedNeighbors;
        /// <summary>
        /// returns neighbors of specific atom
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public List<Atom> Neighbors(Atom a)
        {
            //build cache at first call
            if (CacheNeighborList && (_cachedNeighbors == null || _cachedNeighbors.Count != Atoms.Count))
                _cachedNeighbors = AtomUtil.BuildNeighborCache(Atoms, Bonds);
            return CacheNeighborList ? _cachedNeighbors[a] : AtomUtil.Neighbors(a, this).ToList();
        }

        public void RebuildCache() => _cachedNeighbors = AtomUtil.BuildNeighborCache(Atoms, Bonds);

        /// <summary>
        /// returns nonmetal neighbors of specific atom
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public IEnumerable<Atom> NonMetalNeighbors(Atom a) => Neighbors(a) != null && Neighbors(a).Any() ? Neighbors(a).Where(s => s != null && s.IsNonMetal) : Enumerable.Empty<Atom>();
    }
}
