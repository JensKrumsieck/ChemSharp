using System.Linq;
using ChemSharp.Files.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]
    public class Mol2Test
    {
        public string path = "files/benzene.mol2";
        [TestMethod]
        public void TestMol2Atoms()
        {
            var mol2 = new MOL2(path);
            Assert.AreEqual(12, mol2.Atoms.Count());
            Assert.AreEqual(6, mol2.Atoms.Count(s => s.Symbol == "C"));
            Assert.AreEqual(6, mol2.Atoms.Count(s => s.Symbol == "H"));
        }

        [TestMethod]
        public void TestMol2Bonds()
        {
            var mol2 = new MOL2(path);
            Assert.AreEqual(12, mol2.Bonds.Count());
        }
    }
}
