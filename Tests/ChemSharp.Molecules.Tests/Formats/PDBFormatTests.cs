using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class PDBFormatTests
{
	[Theory]
	[InlineData("files/oriluy.pdb", 130, 151)]
	[InlineData("files/2spl.pdb", 1437, 1314)]
	[InlineData("files/1hv4.pdb", 9288, 9562)]
	public void PDBFormat_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = PDBFormat.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Bonds.Count.Should().Be(bondsCount);
	}

	[Theory]
	[InlineData("files/oriluy.pdb")]
	[InlineData("files/2spl.pdb")]
	[InlineData("files/1hv4.pdb")]
	public void PDBFormat_MatchesLegacyImplementation(string file)
	{
		var mol = PDBFormat.Read(file);
		var old = new PDBDataProvider(file);
		var oldMol = new Molecule(old);
		oldMol.Atoms.Should().BeEquivalentTo(mol.Atoms);
		oldMol.Bonds.Should().BeEquivalentTo(mol.Bonds);
	}
}
