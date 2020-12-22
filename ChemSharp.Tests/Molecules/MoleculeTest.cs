using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class MoleculeTest
    {
        [TestMethod]
        public void TestCif()
        {
            //Mo-Corrole, Cl2 Ligand, OMePh Meso, 79 Atoms, 89 Bonds C40Cl2H29MoN4O3, M=780.51
            const string path = "files/cif.cif";
            var provider = new CIFDataProvider(path);
            var mol = new Molecule() { AtomDataProvider = provider };
            Assert.AreEqual(79, mol.Atoms.Count);
        }
    }
}
