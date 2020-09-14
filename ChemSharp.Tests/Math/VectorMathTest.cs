using System.Linq;
using ChemSharp.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using ChemSharp.Tests.MoleculeFiles;

namespace ChemSharp.Tests.Math
{
    [TestClass]
    public class VectorMathTest
    {
        private Vector3[] _vectors;

        [TestInitialize]
        public void SetUp()
        {
            var v1 = Vector3.UnitX;
            var v2 = Vector3.UnitY;
            var v3 = Vector3.UnitZ;
            _vectors = new[] { v1, v2, v3 };
        }

        [TestMethod]
        public void TestVectorSum()
        {
            Assert.AreEqual(Vector3.One, _vectors.Sum());
        }

        [TestMethod]
        public void TestCentroid()
        {
            Assert.AreEqual(new Vector3(1 / 3f, 1 / 3f, 1 / 3f), _vectors.Centroid());
        }

        [TestMethod]
        public void DistanceToPlane()
        {
            //use XYZ her
            var xyz = XYZTest._xyz;
            //from mercury
            var centroid = new Vector3(.258f, .052f,-0.015f);
            var calcCentroid = xyz.Atoms.Centroid();
            Assert.AreEqual(centroid.X, calcCentroid.X, 0.001f);
            Assert.AreEqual(centroid.Y, calcCentroid.Y, 0.001f);
            Assert.AreEqual(centroid.Z, calcCentroid.Z, 0.001f);
            //Data from mercury
            Assert.AreEqual(0.057, xyz.Atoms.FirstOrDefault(s => s.Symbol == "O").DistanceToMeanPlane(xyz.Atoms), 0.001);
        }
    }
}
