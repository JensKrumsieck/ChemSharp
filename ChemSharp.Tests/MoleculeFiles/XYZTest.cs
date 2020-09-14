using ChemSharp.Files.Molecule;
using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Numerics;
using ChemSharp.Math;

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
        [TestMethod]
        public void DistanceToPlane()
        {
            //from mercury
            var centroid = new Vector3(.258f, .052f, -0.015f);
            var calcCentroid = _xyz.Atoms.Centroid();
            Assert.AreEqual(centroid.X, calcCentroid.X, 0.001f);
            Assert.AreEqual(centroid.Y, calcCentroid.Y, 0.001f);
            Assert.AreEqual(centroid.Z, calcCentroid.Z, 0.001f);
            //Data from mercury
            Assert.AreEqual(0.057, _xyz.Atoms.FirstOrDefault(s => s.Symbol == "O").DistanceToMeanPlane(_xyz.Atoms), 0.001);
        }
    }
}
