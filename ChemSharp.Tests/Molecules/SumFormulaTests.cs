using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class SumFormulaTests
    {
        [TestMethod]
        public void TestFromMolecule()
        {
            const string file = "files/cif.cif";
            var prov = new CIFDataProvider(file);
            Assert.AreEqual("C40Cl2H29MoN4O3", prov.Atoms.SumFormula());
        }

        [TestMethod]
        public void GetElementalAnalysis()
        {
            const string formula = "C40Cl2H29MoN4O3";
            var chn = formula.ElementalAnalysis();
            Assert.AreEqual(61.550, chn["C"]);
            Assert.AreEqual(3.745, chn["H"]);
            Assert.AreEqual(7.178, chn["N"]);
            Assert.AreEqual(12.294, chn["Mo"]);
            Assert.AreEqual(6.150, chn["O"]);
            Assert.AreEqual(9.083, chn["Cl"]);
        }

        [TestMethod]
        public void EAAbbreaviationTest()
        {
            const string formula = "EtOH";
            var parsed = formula.Parse();
            Assert.AreEqual("C2H6O1", parsed);
            var chn = formula.ElementalAnalysis();
            Assert.AreEqual(52.142, chn["C"]);
            Assert.AreEqual(13.127, chn["H"]);
            Assert.AreEqual(34.731, chn["O"]);
        }
    }
}
