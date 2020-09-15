using ChemSharp.Molecule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests
{
    [TestClass]
    public class ElementTest
    {
        public static Element[] Elements { get; } = {
            new Element("B"),
            new Element("Si"),
            new Element("Fe"),
            new Element("Co"),
            new Element("P"),
            new Element("Al"),
            new Element("Ne"),
            new Element("H")
        };

        [TestMethod]
        public void TestMetal()
        {
            var trueIndices = new int[] {2, 3, 5};
            Test("IsMetal", trueIndices);
        }

        [TestMethod]
        public void TestMetalloid()
        {
            var trueIndices = new int[] { 0, 1 };
            Test("IsMetalloid", trueIndices);
        }

        [TestMethod]
        public void TestGas()
        {
            var trueIndices = new int[] { 6 };
            Test("IsGas", trueIndices);
        }

        [TestMethod]
        public void TestNonMetal()
        {
            var trueIndices = new int[] { 4,7 };
            Test("IsNonMetal", trueIndices);
        }

        public void Test(string property, int[] trueIndices)
        {
            var prop = typeof(Element).GetProperties().First(s => s.Name == property);
            for (var i = 0; i < Elements.Length; i++)
            {
                if (trueIndices.Contains(i))
                    Assert.IsTrue((bool)(prop.GetValue(Elements[i]) ?? false));
                else 
                    Assert.IsFalse((bool)(prop.GetValue(Elements[i]) ?? true));
            }
        }

        [TestMethod]
        public void PSETest()
        {
            for (var i = 1; i <= 118; i++)
            {
                var element = ElementDataProvider.Elements.ElementAt(i - 1);
                Assert.AreEqual(i, element.AtomicNumber);
                Assert.IsNotNull(element.Color);
            }
        }
    }
}
