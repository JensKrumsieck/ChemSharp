using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class ExportTest
    {
        const string dir = "tmp/";
        [TestInitialize]
        public void InitDir() => Directory.CreateDirectory(dir);

        [TestMethod]
        public void TestExportXYZ()
        {
            const string path = "files/mescho.xyz";
            var mol = new Molecule(new XYZDataProvider(path));
            const string export = dir + "test.xyz";
            XYZExporter.Export(mol, export);
            var importMol = new Molecule(new XYZDataProvider(export));
            Assert.AreEqual(mol.Atoms.Count, importMol.Atoms.Count);
        }

        [TestCleanup]
        public void RemoveDir() => Directory.Delete(dir, true);
    }
}
