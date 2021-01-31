using System.Linq;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
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
            Assert.IsNull(mol.Bonds);
            mol.BondDataProvider = provider;
            Assert.AreEqual(89, mol.Bonds?.Count);
            Assert.AreEqual(780.51, mol.Atoms.MolecularWeight(), 0.035);
        }

        [TestMethod]
        public void TestCif2()
        {
            //Mo-Corrole, Cl2 Ligand, OMePh Meso, 79 Atoms, 89 Bonds C40Cl2H29MoN4O3, M=780.51
            const string path = "files/cif.cif";
            var provider = new CIFDataProvider(path);
            var mol = new Molecule(provider.Atoms, provider.Bonds);
            Assert.AreEqual(79, mol.Atoms.Count);
            Assert.AreEqual(89, mol.Bonds.Count);
        }

        [TestMethod]
        public void TestMol2()
        {
            const string path = "files/benzene.mol2";
            var provider = new Mol2DataProvider(path);
            var mol = new Molecule(provider.Atoms, provider.Bonds);
            Assert.AreEqual(12, mol.Atoms.Count);
            Assert.AreEqual(12, mol.Bonds.Count);
            Assert.AreEqual(78.11184, mol.Atoms.MolecularWeight(), 0.02);
            //are C-C bonds in benzene aromatic? :D
            foreach (var b in mol.Bonds.Where(b => b.Atoms.Count(c => c.Symbol == "C") == 2))
                Assert.IsTrue(b.IsAromatic);
        }

        [TestMethod]
        public void TestMol2_2()
        {
            const string path = "files/tep.mol2";
            var provider = new Mol2DataProvider(path);
            var mol = new Molecule(provider);
            Assert.AreEqual(46, mol.Atoms.Count);
            Assert.AreEqual(10, mol.Bonds.Count(s => s.IsAromatic));
            Assert.AreEqual(4, mol.Bonds.Count(s => s.Order == 3));
        }

        [TestMethod]
        public void TestXYZ()
        {
            const string path = "files/mescho.xyz";
            var provider = new XYZDataProvider(path);
            var mol = new Molecule(provider.Atoms);
            Assert.AreEqual(23, mol.Atoms.Count);
            mol.RecalculateBonds();
            Assert.AreEqual(23, mol.Bonds.Count);
            Assert.AreEqual(148.205, mol.Atoms.MolecularWeight(), 0.02);
        }

        [TestMethod]
        public void TestXYZ2()
        {
            const string path = "files/mescho.xyz";
            var provider = new XYZDataProvider(path);
            var mol = new Molecule() { AtomDataProvider = provider }; ;
            Assert.AreEqual(23, mol.Atoms.Count);
            Assert.IsNull(mol.Bonds);
        }
    }
}
