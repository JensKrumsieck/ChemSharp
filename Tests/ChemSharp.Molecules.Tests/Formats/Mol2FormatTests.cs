using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class Mol2FormatTests
{
	[Theory, InlineData("files/benzene.mol2", 12, 12), InlineData("files/myo.mol2", 1437, 1312),
	 InlineData("files/tep.mol2", 46, 50), InlineData("files/ptcor.mol2", 129, 127)]
	public void Mol2Format_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = Mol2Format.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Bonds.Count.Should().Be(bondsCount);
	}
}
