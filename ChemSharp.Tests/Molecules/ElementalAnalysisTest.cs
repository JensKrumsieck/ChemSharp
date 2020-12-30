using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.ElementalAnalysis;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class ElementalAnalysisTest
    {


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
            var best = ElementalAnalysisUtil.Solve(formula, experimental, new HashSet<Impurity> { dcm });
            //should be 0.2
            Assert.AreEqual(.2, best[0]);
        }

        [TestMethod]
        public void TestWith3Impurities()
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
            var hexane = new Impurity("C6H14", 0, 1, 0.1);
            var water = new Impurity("H2O", 0, 1, 0.1);
            var best = ElementalAnalysisUtil.Solve(formula, experimental, new HashSet<Impurity> { dcm, hexane, water });
            //should be 0.2
            Assert.AreEqual(.2, best[0]);
        }

        [TestMethod]
        public async Task TestWithClass()
        {
            const string file = "files/cif.cif";
            var mol = new Molecule() { AtomDataProvider = new CIFDataProvider(file) };
            var ea = Analysis.FromMolecule(mol);
            ea.Impurities.Add(new Impurity("CH2Cl2", 0, 1, 0.1));
            ea.Impurities.Add(new Impurity("C6H14", 0, 1, 0.1));
            var experimental = new Dictionary<string, double>
            {
                {"C", 60.55},
                {"H", 2.7},
                {"N", 7.7}
            };
            ea.ExperimentalAnalysis = experimental;
            var best = await ea.Solve();
            //should be 0.2
            Assert.AreEqual(.2, best[0]);
        }
    }
}
