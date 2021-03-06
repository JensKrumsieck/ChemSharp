﻿using ChemSharp.UnitConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Unit
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestEnergy()
        {
            var converter = new EnergyUnitConverter("eV", "cm^-1");
            Assert.AreEqual(8065.544, converter.Convert(1), 0.0005);
            Assert.AreEqual(1 / 8065.544, converter.ConvertInverted(1), 0.0005);
            converter = new EnergyUnitConverter("nm", "cm^-1");
            Assert.AreEqual(20000, converter.Convert(500));
            Assert.AreEqual(500, converter.ConvertInverted(20000));
        }

        [TestMethod]
        public void TestMagnetic()
        {
            var converter = new MagneticUnitConverter("G", "mT");
            Assert.AreEqual(.1, converter.Convert(1));
            Assert.AreEqual(10, converter.ConvertInverted(1));
        }

        [TestMethod]
        public void TestMass()
        {
            var converter = new MassUnitConverter("Solar Mass", "Earth Mass");
            Assert.AreEqual(332946.0487, converter.Convert(1), 1.25);
            converter = new MassUnitConverter("lbs", "g");
            Assert.AreEqual(453.59237, converter.Convert(1));
        }
    }
}
