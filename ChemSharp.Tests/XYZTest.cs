using System.Collections.Generic;
using ChemSharp.Files.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ChemSharp.Molecule;

namespace ChemSharp.Tests
{
    [TestClass]
    public class XYZTest
    {

        public string path = "files/mescho.xyz";

        [TestMethod]
        public void TestXYZAtoms()
        {
            var file = new XYZ(path);
            var atoms = new HashSet<Atom>(file.Atoms);
            Assert.AreEqual(23, atoms.Count());
            Assert.AreEqual("C10H12O", atoms.SumFormula());
            Assert.AreEqual(148.205, atoms.Weight(), 0.025);
        }

        [TestMethod]
        public void TextXYZGeneratedBonds()
        {
            var file = new XYZ(path);
            var mol = new Molecule.Molecule(file.Atoms);
            Assert.AreEqual(23,mol.Bonds.Count());
        }
    }
}
