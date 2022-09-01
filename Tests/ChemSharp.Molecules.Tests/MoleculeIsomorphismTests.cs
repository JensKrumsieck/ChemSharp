using ChemSharp.Molecules.Extensions;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class MoleculeIsomorphism
{
	[Theory, InlineData("files/benzene.mol2", "files/benzene.mol", true),
	 InlineData("files/benzene.mol2", "files/benzene_3d.mol", true),
	 InlineData("files/benzene_arom.mol", "files/benzene.mol", true),
	 InlineData("files/mescho.xyz", "files/benzene.mol", true),
	 InlineData("files/mescho.xyz", "files/benzene_3d.mol", true),
	 InlineData("files/147288.cif", "files/benzene_3d.mol", true),
	 InlineData("files/1484829.cif", "files/benzene_3d.mol", true)]
	public void Subgraphsomorphism_Should_Be_Detected(string file1, string file2, bool isIsomorphic)
	{
		var mol1 = Molecule.FromFile(file1);
		var mol2 = Molecule.FromFile(file2);
		Assert.Equal(mol1.IsSubgraphIsomorphicTo(mol2), isIsomorphic);
	}

	[Theory, InlineData("files/benzene.mol2", "files/benzene.mol", false),
	 InlineData("files/benzene.mol2", "files/benzene_3d.mol", true),
	 InlineData("files/benzene_arom.mol", "files/benzene.mol", false),
	 InlineData("files/benzene_arom.mol", "files/benzene_3d.mol", true),
	 InlineData("files/1484829.cif", "files/benzene_3d.mol", false)]
	public void Isomorphism_Should_Be_Detected(string file1, string file2, bool isIsomorphic)
	{
		var mol1 = Molecule.FromFile(file1);
		var mol2 = Molecule.FromFile(file2);
		Assert.Equal(mol1.IsIsomorphicTo(mol2), isIsomorphic);
	}

	[Theory,
	 InlineData("files/147288.cif", true),
	 InlineData("files/1484829.cif", true),
	 InlineData("files/tep.mol", false),
	 InlineData("files/ptcor.mol2", true),
	 InlineData("files/cif.cif", true), //is a corrole
	 InlineData("files/benzene.mol", false),
	 InlineData("files/oriluy.pdb", false),
	 InlineData("files/ligand.cif", false),
	 InlineData("files/cif_noTrim.cif", true),
	 InlineData("files/VATTOC.mol2", true)] //issue with PS
	public void Corroles_Can_Be_Detected(string file, bool outcome)
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile(file);
		//strip metals, hydrogens and co
		var subCorrole = new Molecule(corrole.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		var subTest = new Molecule(test.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		Assert.Equal(subTest.IsSubgraphIsomorphicTo(subCorrole), outcome);
	}

	[Fact]
	public void Mapping_Is_Plausible()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		//strip metals, hydrogens and co
		var subCorrole = new Molecule(corrole.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		var subTest = new Molecule(test.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		Assert.True(subTest.IsSubgraphIsomorphicTo(subCorrole, out var mapping));
		mapping.Count.Should().Be(23);
	}

	[Fact]
	public void Mapping_Is_Correct()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		//strip metals, hydrogens and co
		var subCorrole = new Molecule(corrole.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		var subTest = new Molecule(test.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		Assert.True(subTest.TryMap(subCorrole, out var result));
		foreach (var (itemInTest, itemInCorrole) in result!)
			itemInTest.Symbol.Should().Be(itemInCorrole.Symbol); //because both are usual corroles;

		//remove result
		var residualTest = new Molecule(subTest.Atoms.Except(result.Select(s => s.atomInTarget)));
		Assert.True(residualTest.TryMap(subCorrole, out result)); //should find a second corrole
		//remove result again
		residualTest = new Molecule(residualTest.Atoms.Except(result!.Select(s => s.atomInTarget)));
		Assert.False(residualTest.IsSubgraphIsomorphicTo(subCorrole)); //should now fail!
	}

	[Fact]
	public void MapAll_Returns_Correct_Mappings()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		//strip metals, hydrogens and co
		var subCorrole = new Molecule(corrole.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		var subTest = new Molecule(test.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		var mappings = subTest.MapAll(subCorrole);
		mappings.Should().HaveCount(23);
		mappings.Values.Should().AllSatisfy(item => item.Count.Should().Be(2));
	}
}
