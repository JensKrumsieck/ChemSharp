using ChemSharp.Extensions;
using ChemSharp.Molecules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemSharp.Tests.Molecules
{
    /// <summary>
    /// Tests DFS Util on Molecules
    /// </summary>
    [TestClass]
    public class MacrocycleDetectionTest
    {
        [TestMethod]
        public async Task DetectCorrole() => await RunTest(@"files\cif.cif", 23);

        [TestMethod]
        public async Task DetectCorroleWithoutCache() => await RunTest(@"files\cif.cif", 23, false);

        //Abraham B. Alemayehu, Hugo Vazquez-Lima, Christine M. Beavers, Kevin J. Gagnon, Jesper Bendix, Abhik Ghosh,
        //Chemical Communications, 2014, 50, 11093,
        //DOI: 10.1039/C4CC02548B
        [TestMethod]
        public async Task DetectPtCorrole() => await RunTest(@"files\ptcor.mol2", 23);

        [TestMethod]
        public async Task DetectPorphyrin() => await RunTest(@"files\tep.mol2", 24);

        [TestMethod]
        public async Task DetectPorphyrinWithoutCache() => await RunTest(@"files\tep.mol2", 24, false);

        [TestMethod]
        public async Task DetectPorphin() => await RunTest(@"files\porphin.cdxml", 24);
        [TestMethod]
        public async Task DetectPorphinWithoutCache() => await RunTest(@"files\porphin.cdxml", 24, false);

        private static async Task RunTest(string path, int size, bool useCache = true)
        {
            var molecule = MoleculeFactory.Create(path);
            molecule.CacheNeighborList = useCache;
            await Task.Run(() => Detect(molecule, size));
        }

        [TestMethod]
        public async Task DetectHeme() => await RunTest(@"files\myo.mol2", 24);

        /// <summary>
        /// Uses <see cref="DFSUtil.ConnectedFigures"/> to  iterate over ConnectedFigures
        /// Tries to find a Macrocyle in a figure and returns
        /// </summary>
        /// <param name="molecule"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static void Detect(Molecule molecule, int size)
        {
            var deadEnds = Element.DesiredSaturation.Where(s => s.Value == 1).Select(s => s.Key).ToList();

            IEnumerable<Atom> NonMetalNonDeadEnd(Atom atom) => molecule.NonMetalNeighbors(atom)?.Where(a => !deadEnds.Contains(a.Symbol));

            var parts = new List<IEnumerable<Atom>>();
            foreach (var fig in DFSUtil.ConnectedFigures(molecule.Atoms.Where(s => molecule.Neighbors(s).Count >= 2),
                NonMetalNonDeadEnd))
            {
                var connected = fig.Distinct().ToArray();
                connected = connected.Where(s => s.IsNonMetal && s.Symbol != "H").ToArray();
                if (connected.Length >= size) parts.Add(connected);
            }

            foreach (var fig in parts.Distinct())
            {
                var data = FindCorpus(fig, NonMetalNonDeadEnd, size);
                if (data?.Count != size) continue;
                Assert.AreEqual(4, data.Count(s => s.Symbol == "N"));
                Assert.AreEqual(size - 4, data.Count(s => s.Symbol == "C"));
            }
        }

        /// <summary>
        /// Finds Rings in Connected Figure
        /// </summary>
        /// <param name="part"></param>
        /// <param name="func"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static HashSet<Atom> FindCorpus(IEnumerable<Atom> part, Func<Atom, IEnumerable<Atom>> func, int size)
        {
            foreach (var atom in part)
            {
                var corpus = FuncRingPath(atom, size - 8, func);
                foreach (var n in corpus.SelectMany(func))
                {
                    var outer = FuncRingPath(n, size - 4, func);
                    outer.UnionWith(corpus);
                    if (outer.Count == size) return outer;
                }
            }
            return null;
        }

        /// <summary>
        /// Uses <see cref="DFSUtil.BackTrack"/> to find Ring
        /// </summary>
        /// <param name="atom"></param>
        /// <param name="size"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static HashSet<Atom> FuncRingPath(Atom atom, int size, Func<Atom, IEnumerable<Atom>> func)
        {
            var goal = func(atom).FirstOrDefault();
            return goal == null ? new HashSet<Atom>() : DFSUtil.BackTrack(atom, goal, func, size);
        }
    }
}
