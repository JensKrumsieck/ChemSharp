using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules;

[TestClass]
public class MoleculeTest
{
    [TestMethod]
    public void TestCif()
    {
        //Mo-Corrole, Cl2 Ligand, OMePh Meso, 79 Atoms, 89 Bonds C40Cl2H29MoN4O3, M=780.51
        const string path = "files/cif.cif";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(79, mol.Atoms.Count);
        Assert.AreEqual(89, mol.Bonds?.Count);
        Assert.AreEqual(780.51, mol.Atoms.MolecularWeight(), 0.035);
    }

    [TestMethod]
    public void TestCif2()
    {
        //Mo-Corrole, Cl2 Ligand, OMePh Meso, 79 Atoms, 89 Bonds C40Cl2H29MoN4O3, M=780.51
        const string path = "files/cif.cif";
        var provider = new CIFDataProvider(path);
        var mol = new Molecule(provider.Atoms, provider.Bonds);
        Assert.AreEqual(79, mol.Atoms.Count);
        Assert.AreEqual(89, mol.Bonds.Count);
    }

    [TestMethod]
    public void TestCif_Trim()
    {
        //same cif, but with whitespaces everywhere
        const string path = "files/cif_noTrim.cif";
        var provider = new CIFDataProvider(path);
        var mol = new Molecule(provider.Atoms, provider.Bonds);
        Assert.AreEqual(79, mol.Atoms.Count);
        Assert.AreEqual(89, mol.Bonds.Count);
    }

    [TestMethod]
    public void TestMol2()
    {
        const string path = "files/benzene.mol2";
        var provider = new Mol2DataProvider(path);
        var mol = new Molecule(provider.Atoms, provider.Bonds);
        Assert.AreEqual(12, mol.Atoms.Count);
        Assert.AreEqual(12, mol.Bonds.Count);
        Assert.AreEqual(78.11184, mol.Atoms.MolecularWeight(), 0.02);
        //are C-C bonds in benzene aromatic?
        foreach (var b in mol.Bonds.Where(b => b.Atoms.Count(c => c.Symbol == "C") == 2))
            Assert.IsTrue(b.IsAromatic);
    }

    [TestMethod]
    public void TestMol2Myo()
    {
        //Myoglobin 2SPL
        //Carver, T.E., Brantley Jr., R.E., Singleton, E.W., Arduini, R.M., Quillin, M.L., Phillips Jr., G.N., Olson, J.S.
        //(1992) J Biol Chem 267: 14443 - 14450
        //http://dx.doi.org/10.1006/jmbi.2001.5028
        //converted \w mercury
        const string path = "files/myo.mol2";
        var provider = new Mol2DataProvider(path);
        var mol = new Molecule(provider.Atoms, provider.Bonds);
        //Mercury counts 1437 atoms and 1312 Bonds
        Assert.AreEqual(mol.Atoms.Count, 1437);
        Assert.AreEqual(mol.Bonds.Count, 1312);
    }


    [TestMethod]
    public void TestPDBMyo()
    {
        //Myoglobin 2SPL
        //Carver, T.E., Brantley Jr., R.E., Singleton, E.W., Arduini, R.M., Quillin, M.L., Phillips Jr., G.N., Olson, J.S.
        //(1992) J Biol Chem 267: 14443 - 14450
        //http://dx.doi.org/10.1006/jmbi.2001.5028
        const string path = "files/2spl.pdb";
        var mol = MoleculeFactory.Create(path);
        //Mercury counts 1437 atoms and 1312 Bonds, auto detect finds 1314
        Assert.AreEqual(mol.Atoms.Count, 1437);
        Assert.AreEqual(mol.Bonds.Count, 1314);
        //discard aminoacids, water, carbon monoxide & sulfate
        var atomsNonAmino = mol.Atoms.Where(s => !PDBDataProvider.AminoAcids.ContainsKey(s.Tag) && s.Tag != "HOH" && s.Tag != "SO4" && s.Tag != "CMO");
        var hem = mol.Atoms.Where(s => s.Tag == "HEM");
        Assert.AreEqual(hem.Count(), atomsNonAmino.Count());
    }

    [TestMethod]
    public void TestPDBHemoglobin()
    {
        //HEMOGLOBIN 1HV4
        //Liang, Y., Hua, Z., Liang, X., Xu, Q., Lu, G.
        //(2001) J Mol Biol 313: 123 - 137
        //http://dx.doi.org/10.1006/jmbi.2001.5028
        const string path = "files/1hv4.pdb";
        var provider = new PDBDataProvider(path);
        var mol = new Molecule(provider.Atoms);
        //Mercury counts 9288 atoms and 9560 Bonds, auto detect finds 9562
        Assert.AreEqual(mol.Atoms.Count, 9288);
        Assert.AreEqual(mol.Bonds.Count, 9562);
    }


    [TestMethod]
    public void TestOriluyPDB()
    {
        //Mercury generated pdb file
        //from:
        //P.Schweyen, M.Hoffmann, J.Krumsieck, B.Wolfram, X.Xie, M.Bröring, Angew.Chem.Int.Ed., 2016, 55, 10118, DOI: 10.1002/anie.201604297.
        const string path = "files/oriluy.pdb";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(mol.Atoms.Count, 130);
    }

    [TestMethod]
    public void TestMol2_2()
    {
        const string path = "files/tep.mol2";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(46, mol.Atoms.Count);
        Assert.AreEqual(10, mol.Bonds.Count(s => s.IsAromatic));
        Assert.AreEqual(4, mol.Bonds.Count(s => s.Order == 3));
    }

    [TestMethod]
    public void TestXYZ()
    {
        const string path = "files/mescho.xyz";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(23, mol.Atoms.Count);
        Assert.AreEqual(23, mol.Bonds.Count);
        Assert.AreEqual(148.205, mol.Atoms.MolecularWeight(), 0.02);
    }

    [TestMethod]
    public void TestXYZStream()
    {
        const string path = "files/mescho.xyz";
        var stream = File.Open(path, FileMode.Open);
        var xyz = new XYZDataProvider(stream);
        var mol = new Molecule(xyz);
        Assert.AreEqual(23, mol.Atoms.Count);
        Assert.AreEqual(23, mol.Bonds.Count);
        Assert.AreEqual(148.205, mol.Atoms.MolecularWeight(), 0.02);
    }

    [TestMethod]
    public async Task TestXYZStreamFactory()
    {
        const string path = "files/mescho.xyz";
        var stream = File.Open(path, FileMode.Open);
        var mol = await MoleculeFactory.CreateFromStreamAsync(stream, Path.GetExtension(path).Substring(1));
        Assert.AreEqual(23, mol.Atoms.Count);
        Assert.AreEqual(23, mol.Bonds.Count);
        Assert.AreEqual(148.205, mol.Atoms.MolecularWeight(), 0.02);
    }

    [TestMethod]
    public void TextMol_Avogadro()
    {
        //mol file from avogadro
        const string path = "files/tep.mol";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(46, mol.Atoms.Count);
        //Assert.AreEqual(10, mol.Bonds.Count(s => s.IsAromatic)); Not supported by avogadro
        Assert.AreEqual(4, mol.Bonds.Count(s => s.Order == 3));
    }

    [TestMethod]
    public void TestMol_ChemSpider_2D()
    {
        //mol file from chemspider
        const string path = "files/benzene.mol";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(6, mol.Atoms.Count);
        Assert.AreEqual(6, mol.Bonds.Count);
        //chemspider 2D mol files do not add implicit hydrogens
    }

    [TestMethod]
    public void TestMol_ChemSpider()
    {
        //mol file from chemspider
        const string path = "files/benzene_3d.mol";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(12, mol.Atoms.Count);
        Assert.AreEqual(12, mol.Bonds.Count);
    }

    [TestMethod]
    public void TestMol_Aromaticity()
    {
        //mol file from chemspider, manually edited
        const string path = "files/benzene_arom.mol";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(12, mol.Atoms.Count);
        Assert.AreEqual(12, mol.Bonds.Count);
        Assert.AreEqual(6, mol.Bonds.Count(s => s.IsAromatic));

        Assert.AreEqual(78.11184, mol.Atoms.MolecularWeight(), 0.02);
        //are C-C bonds in benzene aromatic?
        foreach (var b in mol.Bonds.Where(b => b.Atoms.Count(c => c.Symbol == "C") == 2))
            Assert.IsTrue(b.IsAromatic);
    }

    [TestMethod]
    public void TestMol_ChemSpider_Corrole()
    {
        //mol file from chemspider
        const string path = "files/corrole.mol";
        var mol = MoleculeFactory.Create(path);
        Assert.AreEqual(37, mol.Atoms.Count);
        Assert.AreEqual(41, mol.Bonds.Count);
        //chemspider mol files do not add implicit hydrogens
    }
}
