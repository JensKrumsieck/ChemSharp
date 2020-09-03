using ChemSharp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ChemSharp.Tests
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
            Assert.AreEqual(new Vector3(1/3f, 1/3f, 1/3f), _vectors.Centroid());
        }
    }
}
