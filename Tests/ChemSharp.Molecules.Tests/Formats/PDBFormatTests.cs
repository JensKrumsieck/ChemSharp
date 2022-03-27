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
		var atoms = PDBFormat.Read(file);
		atoms.Count.Should().Be(count);
	}

	[Theory]
	[InlineData("files/oriluy.pdb", 130)]
	[InlineData("files/2spl.pdb", 1437)]
	[InlineData("files/1hv4.pdb", 9288)]
	public void PDBFormat_MatchesLegacyImplementation(string file, int count)
	{
		var atoms = PDBFormat.Read(file);
		var old = new PDBDataProvider(file);
		old.ReadData();
		var old_Atoms = old.Atoms.ToList();
		old_Atoms.ToList().Should().BeEquivalentTo(atoms);
	}
}
