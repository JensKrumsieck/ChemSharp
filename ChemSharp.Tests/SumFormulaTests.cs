using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests
{
    [TestClass]
    public class SumFormulaTests
    {
        [TestMethod]
        public void TestFerrozin() => RunTest("K4[Fe(CN)6]", 17, "C6FeK4N6");

        [TestMethod]
        public void TestPorphine() => RunTest("C20H14N4", 38, "C20H14N4");

        [TestMethod]
        public void TestCarboraneAcid() => RunTest("H(CHB11F11)", 25, "B11CF11H2");

        public void RunTest(string formula, int expectedNumberOfElements, string expectedFormula)
        {
            var elements = formula.ToElements();
            Assert.AreEqual(expectedNumberOfElements, elements.Count());
            var parsed = elements.SumFormula();
            Assert.AreEqual(expectedFormula, parsed);
        }

    }
}
