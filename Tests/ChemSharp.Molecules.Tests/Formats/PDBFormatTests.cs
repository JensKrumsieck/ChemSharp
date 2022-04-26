using System.Linq;
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
		mol.Atoms.Count(s => s.Symbol == "DA").Should().Be(0);
		mol.Bonds.Count.Should().Be(bondsCount);
	}
}
