using ChemSharp.Molecules.Extensions;
using FluentAssertions;
using Nodo.Search;
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
		var subTest = new Molecule(test.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
		Assert.Equal(Prepare(test).IsSubgraphIsomorphicTo(Prepare(corrole)), outcome);
	}

	[Fact]
	public void Mapping_Is_Plausible()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		Assert.True(Prepare(test).IsSubgraphIsomorphicTo(Prepare(corrole), out var mapping));
		mapping.Count.Should().Be(23);
	}

	[Fact]
	public void Mapping_Is_Correct()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		//strip metals, hydrogens and co
		var subCorrole = Prepare(corrole);
		var subTest = Prepare(test);
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

		var mappings = Prepare(test).MapAll(Prepare(corrole));
		mappings.Should().HaveCount(23);
		mappings.Values.Should().AllSatisfy(item => item.Count.Should().Be(2));
	}

	[Theory]
	[InlineData("files/1hv4.pdb", 8)]
	[InlineData("files/2spl.pdb", 1)]
	[InlineData("files/4r21.cif", 2)]
	[InlineData("files/0001.pdb", 24)]
	public void IntegrationTest_With_Proteins(string path, int numberOfHemes)
	{
		var porphyrin = Molecule.FromFile("files/porphyrin.xyz");
		var subPorphyrin = Prepare(porphyrin);

		var protein = Molecule.FromFile(path);
		//in combination with ConnectedFigures() this may be faster!
		var subProtein = new Molecule(protein.Atoms.Where(a =>
			a.IsNonMetal && a.Symbol != "H" && !Constants.AminoAcids.ContainsKey(a.Residue)));

		var mappings = new Dictionary<Atom, List<Atom>>();
		foreach (var test in subProtein.ConnectedFigures().Where(f => f.Count >= 24).Select(f => new Molecule(f)))
		{
			Assert.NotNull(test);
			var tmp = test.MapAll(subPorphyrin);
			foreach (var key in tmp.Keys)
				if (mappings.ContainsKey(key)) mappings[key].AddRange(tmp[key]);
				else mappings[key] = tmp[key];
		}

		mappings.Should().HaveCount(24); //porphyrin has 24 atoms
		mappings.Values.Should().AllSatisfy(item => item.Count.Should().Be(numberOfHemes));
	}

	[Fact]
	public void GetSubgraphs_Should_Return_Individual_Molecules()
	{
		var corrole = Molecule.FromFile("files/corrole.mol");
		var test = Molecule.FromFile("files/147288.cif");
		var matches = Prepare(test).GetSubgraphs(Prepare(corrole)).ToList();
		matches.Should().HaveCount(2);
		matches.Should().AllSatisfy(item => item.Atoms.Should().HaveCount(23));
	}

	public static Molecule Prepare(Molecule mol) => new(mol.Atoms.Where(a => a.IsNonMetal && a.Symbol != "H"));
}
