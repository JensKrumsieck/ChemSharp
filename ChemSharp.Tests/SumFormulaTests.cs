using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests
{
    [TestClass]
    public class SumFormulaTests
    {
        [TestMethod]
        public void TestFerrozin()
        {
            var formula = "K4[Fe(CN)6]";
            var elements = formula.ToElements();
            Assert.AreEqual(17, elements.Count());
            var parsed = elements.SumFormula();
            Assert.AreEqual("C6FeK4N6", parsed);
        }

        [TestMethod]
        public void TestPorphine()
        {
            var formula = "C20H14N4";
            var elements = formula.ToElements();
            Assert.AreEqual(38, elements.Count());
            var parsed = elements.SumFormula();
            Assert.AreEqual("C20H14N4", parsed);
        }

        [TestMethod]
        public void TestCarboraneAcid()
        {
            var formula = "H(CHB11F11)";
            var elements = formula.ToElements();
            Assert.AreEqual(25, elements.Count());
            var parsed = elements.SumFormula();
            Assert.AreEqual("B11CF11H2", parsed);
        }

    }
}
