using ChemSharp.Molecules.Extensions;
using FluentAssertions;
using Nodo.Search;
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

	[Fact]
	public void ToMolecule_Is_Valid()
	{
		var molecule = Molecule.FromFile("files/corrole.mol");
		molecule.Where(a => a.Symbol != "H").Atoms.ToMolecule().Atoms.Should().HaveCount(23);
		molecule.Where(a => a.Symbol != "H").Bonds.ToMolecule().Atoms.Should().HaveCount(23);
	}

	[Fact]
	public void ToMolecules_Is_Valid()
	{
		var molecule = Molecule.FromFile("files/147288.cif");
		molecule.ConnectedFigures().ToMolecules().Should().HaveCount(6);
	}
}
