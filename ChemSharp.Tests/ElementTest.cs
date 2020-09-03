using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]
    public class ElementTest
    {
        private static readonly Element b = new Element("B");
        private static readonly Element si = new Element("Si");
        private static readonly Element fe = new Element("Fe");
        private static readonly Element co = new Element("Co");
        private static readonly Element p = new Element("P");
        private static readonly Element al = new Element("Al");
        private static readonly Element ne = new Element("Ne");
        private static readonly Element he = new Element("He");

        [TestMethod]
        public void TestMetal()
        {
            Assert.IsFalse(b.IsMetal);
            Assert.IsFalse(si.IsMetal);
            Assert.IsTrue(fe.IsMetal);
            Assert.IsTrue(co.IsMetal);
            Assert.IsFalse(p.IsMetal);
            Assert.IsTrue(al.IsMetal);
            Assert.IsFalse(ne.IsMetal);
            Assert.IsFalse(he.IsMetal);
        }

        [TestMethod]
        public void TestMetalloid()
        {
            Assert.IsTrue(b.IsMetalloid);
            Assert.IsTrue(si.IsMetalloid);
            Assert.IsFalse(fe.IsMetalloid);
            Assert.IsFalse(co.IsMetalloid);
            Assert.IsFalse(p.IsMetalloid);
            Assert.IsFalse(al.IsMetalloid);
            Assert.IsFalse(ne.IsMetalloid);
            Assert.IsFalse(he.IsMetalloid);
        }

        [TestMethod]
        public void TestGas()
        {
            Assert.IsFalse(b.IsGas);
            Assert.IsFalse(si.IsGas);
            Assert.IsFalse(fe.IsGas);
            Assert.IsFalse(co.IsGas);
            Assert.IsFalse(p.IsGas);
            Assert.IsFalse(al.IsGas);
            Assert.IsTrue(ne.IsGas);
            Assert.IsTrue(he.IsGas);
        }

        [TestMethod]
        public void TestNonMetal()
        {
            Assert.IsFalse(b.IsNonMetal);
            Assert.IsFalse(si.IsNonMetal);
            Assert.IsFalse(fe.IsNonMetal);
            Assert.IsFalse(co.IsNonMetal);
            Assert.IsTrue(p.IsNonMetal);
            Assert.IsFalse(al.IsNonMetal);
            Assert.IsFalse(ne.IsNonMetal);
            Assert.IsFalse(he.IsNonMetal);
        }
    }
}
