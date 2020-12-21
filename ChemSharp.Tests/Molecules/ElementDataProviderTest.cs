using System.Linq;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class ElementDataProviderTest
    {
        [TestMethod]
        public void TestColorLoad()
        {
            var col = ElementDataProvider.ColorData;
            Assert.AreEqual(118, col.Count);
        }

        [TestMethod]
        public void TestElementLoad()
        {
            var data = ElementDataProvider.ElementData;
            Assert.AreEqual(118, data.Count());
        }

        [TestMethod]
        public void TestElements()
        {
            var element = new Element("H");
            Assert.AreEqual("H", element.Symbol);
            Assert.AreEqual(ElementDataProvider.ColorData["H"], element.Color);
        }

    }
}
