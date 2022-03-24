using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class AtomTests
{
	[Theory, ClassData(typeof(AtomTestDataGenerator))]
	public void Atom_ShouldHaveSymbol_IfValid(string symbol)
	{
		var atom = new Atom(symbol);
		Assert.Equal(symbol, atom.Symbol);
		Assert.Equal(Vector3.Zero, atom.Location); //default
	}

	[Fact]
	public void Atom_ShouldBeDummy_IfInvalidSymbol()
	{
		var atom = new Atom("FAIL");
		Assert.Equal("DA", atom.Symbol);
		Assert.Equal("Dummy Atom", atom.Name);
		Assert.Equal(Vector3.Zero, atom.Location); //default
	}

	[Fact]
	public void Atom_ShouldNotBeNull()
	{
		var atom1 = new Atom("He");
		Assert.True(atom1 != null);
	}

	[Fact]
	public void Atom_ShouldBeNull()
	{
		Atom atom1 = null!;
		Assert.True(atom1 == null);
	}

	[Fact]
	public void Atoms_ShouldBeEqual_IfSymbolAndLocationMatch()
	{
		var atom1 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		Assert.True(atom1.Equals(atom2));
	}

	[Fact]
	public void Atoms_ShouldNotBeEqual_IfLocationsDoNotMatch()
	{
		var atom1 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("He") {Location = new Vector3(1, 2, 4)};
		Assert.True(!atom1.Equals(atom2));
	}

	[Fact]
	public void Atoms_ShouldNotBeEqual_IfSymbolsDoNotMatch()
	{
		var atom1 = new Atom("H") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		Assert.True(!atom1.Equals(atom2));
	}

	[Fact]
	public void Atoms_ShouldBeEqual_ViaOperator()
	{
		var atom1 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		Assert.True(atom1 == atom2);
	}

	[Fact]
	public void Atoms_ShouldNotBeEqual_IfLocationsDoNotMatch_ViaOperator()
	{
		var atom1 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("He") {Location = new Vector3(1, 2, 4)};
		Assert.True(atom1 != atom2);
	}

	[Fact]
	public void Atoms_ShouldNotBeEqual_IfSymbolsDoNotMatch_ViaOperator()
	{
		var atom1 = new Atom("He") {Location = new Vector3(1, 2, 3)};
		var atom2 = new Atom("Ne") {Location = new Vector3(1, 2, 3)};
		Assert.True(atom1 != atom2);
	}
}

public class AtomTestDataGenerator : IEnumerable<object[]>
{
	private const string AtomSymbols = "H He Li Be B C N O F Ne " +
	                                   "Na Mg Al Si P S Cl Ar K Ca Sc Ti V Cr Mn Fe Co Ni Cu Zn Ga Ge As Se Br Kr " +
	                                   "Rb Sr Y Zr Nb Mo Tc Ru Rh Pd Ag Cd In Sn Sb Te I Xe " +
	                                   "Cs Ba La Hf Ta W Re Os Ir Pt Au Hg Tl Pb Bi Po At Rn " +
	                                   "Fr Ra Ac Rf Db Sg Bh Hs Mt Ds Rg Cn Nh Fl Mc Lv Ts Og " +
	                                   "Ce Pr Nd Pm Sm Eu Gd Tb Dy Ho Er Tm Yb Lu " +
	                                   "Th Pa U Np Pu Am Cm Bk Cf Es Fm Md No Lr ";

	private List<object[]> Data => AtomSymbols
	                               .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
	                               .Select(s => new object[] {s})
	                               .ToList();

	public IEnumerator<object[]> GetEnumerator() => Data.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
