using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class CifFormatTests
{
	[Theory,
	 InlineData("files/cif.cif", 79, 89),
	 InlineData("files/cif_noTrim.cif", 79, 89),
	 InlineData("files/mmcif.cif", 1291, 1251),
	 InlineData("files/ligand.cif", 44, 46),
	 InlineData("files/147288.cif", 206, 230),
	 InlineData("files/4r21.cif", 6752, 6892),
	 InlineData("files/1484829.cif", 466, 528),
	 InlineData("files/CuHETMP.cif", 85, 92)]
	public void CifFormat_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = CifFormat.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Bonds.Count.Should().Be(bondsCount);
	}
}
