using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class ChemDrawTest
    {
        [TestMethod]
        public void TestCDXMLTo3D()
        {
            const string path = "files/porphin.cdxml";
            var provider = new CDXMLDataProvider(path);
            var mol = new Molecule() { AtomDataProvider = provider, BondDataProvider = provider};
            // C20H14N4
            Assert.AreEqual(38, mol.Atoms.Count);
            Assert.AreEqual("C20H14N4", mol.Atoms.SumFormula());
            Assert.AreEqual(42, mol.Bonds.Count);
        }
    }
}
