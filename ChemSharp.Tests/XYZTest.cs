﻿using ChemSharp.Files.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests
{
    [TestClass]
    public class XYZTest
    {

        public string path = "files/mescho.xyz";

        [TestMethod]
        public void XYZLoad()
        {
            var file = new XYZ(path);
            Assert.AreEqual(23, file.Atoms.Count());
        }
    }
}
