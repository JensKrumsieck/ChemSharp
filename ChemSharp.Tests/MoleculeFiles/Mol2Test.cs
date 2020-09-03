using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests.MoleculeFiles
{
    [TestClass]
    public class Mol2Test
    {
        private const string path = "files/benzene.mol2";

        private static readonly MOL2 _mol2 = new MOL2(path);
        private Atom[] _atoms;
        private Bond[] _bonds;

        [TestInitialize]
        public void Setup()
        {
            _atoms = _mol2.Atoms.ToArray();
            _bonds = _mol2.Bonds.ToArray();
        }

        [TestMethod]
        public void TestMol2Atoms()
        {
            Assert.AreEqual(12, _atoms.Length);
            Assert.AreEqual(6, _atoms.Count(s => s.Symbol == "C"));
            Assert.AreEqual(6, _atoms.Count(s => s.Symbol == "H"));
        }

        [TestMethod]
        public void TestMol2Bonds()
        {
            Assert.AreEqual(12, _bonds.Length);
        }
    }
}
