using System.Linq;
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
            var atoms = cif.Atoms.ToArray();
            Assert.AreEqual(780.51, atoms.Weight(), 0.05);
            Assert.AreEqual("C40Cl2H29MoN4O3", atoms.SumFormula());
        }
    }
}
