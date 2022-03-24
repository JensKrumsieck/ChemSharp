using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChemSharp.Molecules.Extensions;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class BondTests
{
	[Fact]
	public void Bond_ShouldHave2Atoms()
	{
		var atoms = new List<Atom> {new("He", 1, 2, 3), new("He", 3, 2, 1)};
		var bond = new Bond(atoms[0], atoms[1]);
		Assert.Equal(2, bond.Atoms.Length);
	}

	[Fact]
	public void Bond_ShouldHaveLength()
	{
		var atoms = new List<Atom> {new("He", 1, 0, 0), new("He", 0, 0, 0)};
		var bond = new Bond(atoms[0], atoms[1]);
		Assert.Equal(1, bond.Length);
	}

	[Fact]
	public void Bond_HasOrder()
	{
		var bond = new Bond(null!, null!) {Order = 4};
		Assert.Equal(4, bond.Order);
	}

	[Fact]
	public void Bond_ShouldBeAromaticInBenzene()
	{
		var molecule = MoleculeFactory.Create("files/benzene_arom.mol");
		var bonds = molecule.Bonds
		                    .Where(b => b.Atoms.All(s => s.Symbol == "C"));

		Assert.All(bonds, b => Assert.True(b.IsAromatic));
	}

	[Fact]
	public void Bond_AtomWithNoNeighborsReturnsEmpty()
	{
		var atom = new Atom("Nb");
		var n = AtomUtil.Neighbors(atom, new List<Bond>(0));
		Assert.NotNull(n);
		Assert.Empty(n);
	}

	[Fact]
	public void Bond_NonMetalNeighborsIsNull_IfOnlyMetalsPresent()
	{
		var atoms = new List<Atom> {new("He", 1, 2, 3), new("Ni", 2, 2, 3), new("Co", 2, 3, 3), new("Fe", 2, 2, 4)};
		var bonds = new List<Bond> {new(atoms[0], atoms[1]), new(atoms[2], atoms[3])};
		var mol = new Molecule(atoms, bonds);
		var n = mol.NonMetalNeighbors(atoms[0]);
		Assert.NotNull(n);
		Assert.Empty(n);
	}

	[Fact]
	public void Bond_WithNoNeighborsDoesNotFail()
	{
		var atom = new Atom("Nb");
		var mol = new Molecule(new[] {atom}, new List<Bond>(0));
		var n = mol.NonMetalNeighbors(atom);
		Assert.NotNull(n);
		Assert.Empty(n);
	}

	/// <summary>
	///     Had an error before
	/// </summary>
	[Fact]
	public void Bond_PtCorroleHasCorrectNumberOfBonds()
	{
		const string file = "files/ptcor.mol2";

		var mol = MoleculeFactory.Create(file);
		//remember caching is on!
		foreach (var a in mol.Atoms)
		{
			var noMNeighbors = mol.NonMetalNeighbors(a);
			Assert.NotNull(noMNeighbors);
			var neighbors = mol.Neighbors(a);
			Assert.NotNull(neighbors);
			Assert.Equal(neighbors.Count, AtomUtil.Neighbors(a, mol.Bonds).Count());
		}
	}

	/// <summary>
	///     fixed an error where bonds vanish, when atoms are renamed...
	/// </summary>
	[Fact]
	public void Bond_DoNotVanishBug()
	{
		var atoms = new List<Atom>
		{
			new("He") {Location = new Vector3(1, 2, 3)},
			new("He") {Location = new Vector3(2, 2, 3)},
			new("He") {Location = new Vector3(2, 3, 3)},
			new("He") {Location = new Vector3(2, 2, 4)}
		};
		var bonds = new List<Bond> {new(atoms[0], atoms[1]), new(atoms[2], atoms[3])};
		Assert.Equal(2, bonds.Count);
		atoms[0].Title = "CHANGED!";
		Assert.Equal(2, bonds.Count);
		atoms[1].Title = "CHANGED";
		Assert.Equal(2, bonds.Count);
		foreach (var b in bonds)
			Assert.Equal(2, b.Atoms.Length);

		foreach (var a in atoms)
			Assert.Single(AtomUtil.Neighbors(a, bonds));
	}
}
