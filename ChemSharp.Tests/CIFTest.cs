using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ChemSharp.Tests
{
    [TestClass]

    public class CIFTest
    {
        //Mo-Corrole, Cl2 Ligand, OMePh Meso
        public string path = "files/cif.cif";

        [TestMethod]
        public void TestCIFAtoms()
        {
            var cif = new CIF(path);
            var atoms = new HashSet<Atom>(cif.Atoms);
            Assert.AreEqual(780.51, atoms.Weight(), 0.05);
            Assert.AreEqual("C40Cl2H29MoN4O3", atoms.SumFormula());
            Assert.AreEqual(79,atoms.Count);
        }

        [TestMethod]
        public void TestCIFBonds()
        {
            var cif = new CIF(path);
            var bonds = new HashSet<Bond>(cif.Bonds);
            Assert.AreEqual(89, bonds.Count);
        }
    }
}
