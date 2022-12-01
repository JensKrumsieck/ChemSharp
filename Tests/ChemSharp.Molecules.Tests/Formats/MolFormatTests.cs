using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class MolFormatTests
{
	[Theory, InlineData("files/benzene_3d.mol", 12, 12),
	 InlineData("files/benzene_arom.mol", 12, 12),
	 InlineData("files/benzene.mol", 6, 6)]
	public void MolFormat_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = MolFormat.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Atoms.Count(s => s.Symbol == "DA").Should().Be(0);
		mol.Bonds.Count.Should().Be(bondsCount);
	}

	[Fact]
	public void MolFormat_CanReadAromatics()
	{
		const string file = "files/benzene_arom.mol";
		var mol = MolFormat.Read(file);
		mol.Bonds.Count(s => s.IsAromatic).Should().Be(6);
	}
}
