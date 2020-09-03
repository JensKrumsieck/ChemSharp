using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ChemSharp.Files.Molecule;
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
            Debug.WriteLine(mol2.Atoms.First().GetHashCode());
            _molecule = new Molecule.Molecule(mol2.Atoms, mol2.Bonds);
            Debug.WriteLine(_molecule.Atoms.First().GetHashCode());
        }

        [TestMethod]
        public void TestIsBond()
        {
            var c1 = _molecule.Atoms.ElementAt(0);
            Debug.WriteLine(c1.GetHashCode());
            var c2 = _molecule.Atoms.ElementAt(1);
            Assert.IsTrue(_molecule.IsBond(c1,c2));
        }

    }
}
