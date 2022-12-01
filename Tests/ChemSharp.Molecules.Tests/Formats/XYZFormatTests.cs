using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class XYZFormatTests
{
	[Theory, InlineData("files/cif.xyz", 102, 155), InlineData("files/mescho.xyz", 23, 23)]
	public void XYZFormat_ShouldBeValid(string path, int atoms, int bonds)
	{
		var mol = XYZFormat.Read(path);
		mol.Atoms.Count.Should().Be(atoms);
		mol.Bonds.Count.Should().Be(bonds);
	}
}
