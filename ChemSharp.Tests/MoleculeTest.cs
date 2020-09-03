using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    /// <summary>
    /// Tests Molecule Methods
    /// </summary>
    [TestClass]
    public class MoleculeTest
    {
        public string path = "files/benzene.mol2";
        private Molecule.Molecule _molecule;

        [TestInitialize]
        public void SetUp()
        {
            var mol2 = new MOL2(path);
            _molecule = new Molecule.Molecule(mol2.Atoms, mol2.Bonds);
        }

        [TestMethod]
        public void FactoryCreationTest()
        {
            var mol = MoleculeFactory.Create(path);
            Assert.AreEqual(_molecule.Centroid, mol.Centroid);
            Assert.AreEqual(_molecule.Atoms.Count(), mol.Atoms.Count());
            Assert.AreEqual(_molecule.Bonds.Count(), mol.Bonds.Count());
        }

        [TestMethod]
        public void TestIsBond()
        {
            var c1 = _molecule.Atoms.ElementAt(0);
            var c2 = _molecule.Atoms.ElementAt(1);
            Assert.IsTrue(_molecule.IsBond(c1,c2));
        }

    }
}
