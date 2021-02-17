using ChemSharp.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Misc
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void TestL2Norm()
        {
            var vec = new[] { 1.5d, 2d, 0.4d, 29d, 7.175d };
            var nVec = vec.Normalize();
            Assert.AreEqual(1, nVec.Length());
        }
    }
}
