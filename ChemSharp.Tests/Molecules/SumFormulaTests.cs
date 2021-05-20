using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            var err = ElementalAnalysisUtil.Error(ref chn, ref experimental);
            Assert.AreEqual(16.2595931744669, err, .05);
        }

        /// <summary>
        /// Test cases from http://rosettacode.org/wiki/Chemical_calculator#C.23
        /// </summary>
        [TestMethod]
        public void RosettaTest()
        {
            var molecules = new[]{
                "H", "H2", "H2O", "H2O2", "(HO)2", "Na2SO4", "C6H12",
                 "C6H4O2(OH)4", "C27H46O","COOH(C(CH3)2)3CH3", "(H4O2)0.5", "(H0.5O0.5)4"
            };
            var masses = new[]
            {
                1.008, 2.016, 18.015, 34.014, 34.014, 142.036, 84.162,  176.124, 386.664,186.295, 18.015, 34.014
            };

            for (var i = 0; i < molecules.Length; i++)
            {
                var mass = molecules[i].MolecularWeight();
                Assert.AreEqual(mass, masses[i], .1,
                    $"Failed for {mass} and {masses[i]} with molecule {molecules[i]}");
            }
        }
    }
}
