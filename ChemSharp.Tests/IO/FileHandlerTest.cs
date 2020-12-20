using ChemSharp.Files;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.IO
{
    [TestClass]
    public class FileHandlerTest
    {
        [TestMethod]
        public void TestEprFiles()
        {
            //not necessary if added by ProviderTest.cs
            if (!FileHandler.RecipeDictionary.ContainsKey("par")) FileHandler.RecipeDictionary.Add("par", s => new ParameterFile(s, " "));
            if (!FileHandler.RecipeDictionary.ContainsKey("spc")) FileHandler.RecipeDictionary.Add("spc", s => new PlainFile<float>(s));

            const string par = "files/epr.par";
            var parFile = (ParameterFile)FileHandler.Handle(par);
            Assert.AreEqual(par, parFile?.Path);

            //this file has 23 lines
            Assert.AreEqual(23, parFile?.Content.Length);

            const string spc = "files/epr.spc";
            var spcFile = (PlainFile<float>)FileHandler.Handle(spc);
            Assert.AreEqual(2048, spcFile.Content.Length);
        }
    }
}
