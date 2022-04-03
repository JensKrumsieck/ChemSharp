using System.Linq;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class PDBFormatTests
{
	[Theory]
	[InlineData("files/oriluy.pdb", 130)]
	[InlineData("files/2spl.pdb", 1437)]
	[InlineData("files/1hv4.pdb", 9288)]
	public void PDBFormat_CanReadPlausibleData(string file, int count)
	{
		var mol = PDBFormat.Read(file);
		mol.Atoms.Count.Should().Be(count);
	}

	[Theory]
	[InlineData("files/oriluy.pdb")]
	[InlineData("files/2spl.pdb")]
	[InlineData("files/1hv4.pdb")]
	public void PDBFormat_MatchesLegacyImplementation(string file)
	{
		var mol = PDBFormat.Read(file);
		var old = new PDBDataProvider(file);
		var oldAtoms = old.Atoms.ToList();
		oldAtoms.ToList().Should().BeEquivalentTo(mol.Atoms);
	}
}
