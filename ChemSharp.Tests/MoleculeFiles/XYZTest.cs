using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests.MoleculeFiles
{
    [TestClass]
    public class XYZTest
    {

        private const string path = "files/mescho.xyz";

        public static readonly XYZ _xyz = new XYZ(path);
        private Atom[] _atoms;
        private Bond[] _bonds;

        [TestInitialize]
        public void SetUp()
        {
            _atoms = _xyz.Atoms.ToArray();
            var mol = new Molecule.Molecule(_atoms);
            _bonds = mol.Bonds.ToArray();
        }

        [TestMethod]
        public void TestXYZAtoms()
        {
            Assert.AreEqual(23, _atoms.Length);
            Assert.AreEqual("C10H12O", _atoms.SumFormula());
            Assert.AreEqual(148.205, _atoms.Weight(), 0.025);
        }

        [TestMethod]
        public void TextXYZGeneratedBonds()
        {
            Assert.AreEqual(23, _bonds.Length);
        }
    }
}
