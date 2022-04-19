using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class CifFormatTests
{
	[Theory, InlineData("files/cif.cif", 79, 89), InlineData("files/cif_noTrim.cif", 79, 89)]
	public void CifFormat_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = CifFormat.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Bonds.Count.Should().Be(bondsCount);
	}
}
