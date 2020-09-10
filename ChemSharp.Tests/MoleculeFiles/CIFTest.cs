using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Tests.MoleculeFiles
{
    [TestClass]

    public class CIFTest
    {
        //Mo-Corrole, Cl2 Ligand, OMePh Meso
        private const string path = "files/cif.cif";

        private static readonly CIF _cif = new CIF(path);
        private HashSet<Atom> _atoms;
        private HashSet<Bond> _bonds;

        [TestMethod]
        public void TestCIFAtoms()
        {
            _atoms = _cif.Atoms.ToHashSet();
            Assert.AreEqual(79, _atoms.Count);
            Assert.AreEqual(780.51, _atoms.Weight(), 0.05);
            Assert.AreEqual("C40Cl2H29MoN4O3", _atoms.SumFormula());
        }

        [TestMethod]
        public void TestCIFBonds()
        {
            _bonds = _cif.Bonds.ToHashSet();
            Assert.AreEqual(89, _bonds.Count);
        }
    }
}
