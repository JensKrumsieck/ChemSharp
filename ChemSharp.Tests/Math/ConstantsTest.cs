using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Math
{
    [TestClass]
    //Test calculated constants
    //BohrRadius and Hartree imply that reduced planck is correct!
    public class ConstantsTest
    {
        [TestMethod]
        public void TestBohrRadius()
        {
            Assert.AreEqual(5.291772109e-11, Constants.BohrRadius, 1e-20);
        }

        [TestMethod]
        public void TestBohrMagneton()
        {
            Assert.AreEqual(9.2740100783e-24, Constants.BohrM, 1e-34);
        }

        [TestMethod]
        public void TestHartree()
        {
            Assert.AreEqual(4.3597447222071e-18, Constants.Hartree, 1e-28);
        }

        [TestMethod]
        public void TestGasConstant()
        {
            Assert.AreEqual(8.31446261815324, Constants.GasConstant, 1e-4);
        }
    }
}
