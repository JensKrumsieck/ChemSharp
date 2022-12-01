using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Mathematics;

public class CoordinatesTest
{
	[Fact]
	public void FractionalCoordinates()
	{
		//cif uses conversion from fractional to cartesian, xyz is already cartesian
		var cif = Molecule.FromFile("files/cif.cif");
		var xyz = Molecule.FromFile("files/cif.xyz");
		for (var i = 0; i < cif.Atoms.Count; i++)
		{
			cif.Atoms[i].Location.X.Should().BeApproximately(xyz.Atoms[i].Location.X, .0001f);
			cif.Atoms[i].Location.Y.Should().BeApproximately(xyz.Atoms[i].Location.Y, .0001f);
			cif.Atoms[i].Location.Z.Should().BeApproximately(xyz.Atoms[i].Location.Z, .0001f);
		}
	}
}
