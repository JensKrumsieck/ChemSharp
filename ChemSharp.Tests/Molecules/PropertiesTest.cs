using ChemSharp.Molecules;
using ChemSharp.Molecules.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class PropertiesTest
    {
        [TestMethod]
        public void TestDistance()
        {
            var atom1 = new Atom("He") { Location = new Vector3(1, 0, 0) };
            var atom2 = new Atom("Pr") { Location = new Vector3(0, 0, 0) };
            var prop = new Distance(atom1, atom2);
            Assert.AreEqual(1f, prop.Value);
        }

        [TestMethod]
        public void TestAngle()
        {
            var atom1 = new Atom("He") { Location = new Vector3(1, 0, 0) };
            var atom2 = new Atom("Pr") { Location = new Vector3(0, 0, 0) };
            var atom3 = new Atom("Nb") { Location = new Vector3(0, 1, 0) };
            var prop = new Angle(atom1, atom2, atom3);
            Assert.AreEqual(90f, prop.Value);
        }

        [TestMethod]
        public void TestDihedral()
        {
            var atom1 = new Atom("He") { Location = new Vector3(1, 0, 0) };
            var atom2 = new Atom("Pr") { Location = new Vector3(0, 0, 0) };
            var atom3 = new Atom("Nb") { Location = new Vector3(0, 1, 0) };
            var atom4 = new Atom("Lu") { Location = new Vector3(0, 1, 1) };
            var prop = new Dihedral(atom1, atom2, atom3, atom4);
            Assert.AreEqual(-90f, prop.Value);
        }
    }
}
