using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Tests
{
    [TestClass]

    public class CIFTest
    {
        //Mo-Corrole, Cl2 Ligand, OMePh Meso
        public string path = "files/cif.cif";

        private CIF _cif;
        private Atom[] _atoms;
        private Bond[] _bonds;

        [TestInitialize]
        public void Setup()
        {
            _cif = new CIF(path); 
            _atoms = _cif.Atoms.ToArray();
            _bonds = _cif.Bonds.ToArray();
        }

        [TestMethod]
        public void TestCIFAtoms()
        {
            Assert.AreEqual(79, _atoms.Length);
            Assert.AreEqual(780.51, _atoms.Weight(), 0.05);
            Assert.AreEqual("C40Cl2H29MoN4O3", _atoms.SumFormula());
        }

        [TestMethod]
        public void TestCIFBonds()
        {
            Assert.AreEqual(89, _bonds.Length);
        }
    }
}
