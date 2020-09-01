using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]

    public class CIFTest
    {
        public string path = "files/cif.cif";

        [TestMethod]
        public void TestLoadCIF()
        {
            var cif = new CIF(path);
            Assert.AreEqual(780.51, cif.Atoms.Weight(), 0.05);
            Assert.AreEqual("C40Cl2H29MoN4O3", cif.Atoms.SumFormula());
        }
    }
}
