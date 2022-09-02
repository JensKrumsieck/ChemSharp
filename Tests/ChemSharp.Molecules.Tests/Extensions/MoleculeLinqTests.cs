using ChemSharp.Molecules.Extensions;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Extensions;

public class MoleculeLinqTests
{
	[Fact]
	public void Where_Is_Valid()
	{
		var molecule = Molecule.FromFile("files/corrole.mol");

		molecule.Where(a => a.IsMetal).Atoms.Should().BeEmpty();
		molecule.Where(a => a.IsMetal).Bonds.Should().BeEmpty();

		molecule.Where(a => a.Symbol != "H").Atoms.Should().HaveCount(23);
		molecule.Where(a => a.Symbol != "H").Bonds.Should().HaveCount(27);
	}
}
