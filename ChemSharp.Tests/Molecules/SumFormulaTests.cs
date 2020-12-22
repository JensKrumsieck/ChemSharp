using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
        public void EAAbbreviationTest()
        {
            const string formula = "EtOH";
            var parsed = formula.Parse();
            Assert.AreEqual("C2H6O1", parsed);
            var chn = formula.ElementalAnalysis();
            Assert.AreEqual(52.142, chn["C"]);
            Assert.AreEqual(13.127, chn["H"]);
            Assert.AreEqual(34.731, chn["O"]);
        }

        [TestMethod]
        public void EAAbbreviationTest2()
        {
            const string formula = "Fe(acac)3";
            var parsed = formula.Parse();
            Assert.AreEqual("Fe1C15H21O6", parsed);
        }

        [TestMethod]
        public void DeviationTest()
        {
            const string formula = "PhH";
            var parsed = formula.Parse();
            Assert.AreEqual("C6H6", parsed);
            var chn = formula.ElementalAnalysis();
            var experimental = new Dictionary<string, double>
            {
                {"C", 90},
                {"H", 10}
            };
            const double delta = 2.258;
            var deviation = ElementalAnalysisUtil.Deviation(chn, experimental);
            Assert.AreEqual(delta, deviation["C"]);
            Assert.AreEqual(delta, deviation["H"]);
            var err = ElementalAnalysisUtil.Error(chn, experimental);
            Assert.AreEqual(16.2595931744669, err, .05);
        }

        [TestMethod]
        public void TestSimulation()
        {
            const string file = "files/cif.cif";
            var prov = new CIFDataProvider(file);
            var formula = prov.Atoms.SumFormula();
            var chn = formula.ElementalAnalysis();
            var experimental = new Dictionary<string, double>
            {
                {"C", 60.55},
                {"H", 2.7},
                {"N", 7.7}
            };
            var deviation = ElementalAnalysisUtil.Deviation(chn, experimental);
            Assert.AreEqual(1.00, deviation["C"]);
            var dcm = new Impurity("CH2Cl2", 0, 1, 0.1);
            var best = ElementalAnalysisUtil.Solve(formula, experimental, new List<Impurity> {dcm});
            //should be 0.2
            Assert.AreEqual(.2, best[0]);
        }
    }
}
