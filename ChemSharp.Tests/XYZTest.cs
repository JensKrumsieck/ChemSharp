using ChemSharp.Files.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ChemSharp.Molecule;

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
            Assert.AreEqual("C10H12O", file.Atoms.SumFormula());
            Assert.AreEqual(148.205, file.Atoms.Weight(), 0.025);
        }
    }
}
