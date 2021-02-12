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
        public async Task DetectPorphyrin() => await RunTest(@"files\tep.mol2", 24);

        [TestMethod]
        public async Task DetectPorphin() => await RunTest(@"files\porphin.cdxml", 24);

        private static async Task RunTest(string path, int size)
        {
            var molecule = MoleculeFactory.Create(path);
            await Detect(molecule, size);
        }

        /// <summary>
        /// Uses <see cref="DFSUtil.ConnectedFigures"/> to  iterate over ConnectedFigures
        /// Tries to find a Macrocyle in a figure and returns
        /// </summary>
        /// <param name="molecule"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static async Task Detect(Molecule molecule, int size)
        {
            var deadEnds = Element.DesiredSaturation.Where(s => s.Value == 1).Select(s => s.Key).ToList();
            IEnumerable<Atom> NonMetalNonDeadEnd(Atom atom) => molecule.NonMetalNeighbors(atom)?.Where(a => !deadEnds.Contains(a.Symbol));
            var figures = await DFSUtil.ConnectedFigures(molecule.Atoms.Where(s => molecule.Neighbors(s).Count() >= 2),
                molecule.NonMetalNeighbors).ToListAsync();
            foreach (var fig in figures.Distinct())
            {
                var data = FindCorpus(fig, NonMetalNonDeadEnd, size);
                if (data.Count != size) continue;
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
