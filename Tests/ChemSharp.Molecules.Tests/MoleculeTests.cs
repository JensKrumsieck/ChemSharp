using System.Collections;
using System.Collections.Generic;
using ChemSharp.Molecules.Extensions;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class MoleculeTests
{
	[Theory, ClassData(typeof(MoleculeTestDataGenerator))]
	public void Molecule_ShouldBeCreatedViaFactory(string path, int atoms, int bonds, string? formula, double? weight)
	{
		var mol = MoleculeFactory.Create(path);
		Assert.Equal(atoms, mol.Atoms.Count);
		Assert.Equal(bonds, mol.Bonds.Count);
		if (!string.IsNullOrEmpty(formula)) Assert.Equal(formula, mol.Atoms.SumFormula());

		if (weight.HasValue) Assert.Equal(weight.Value, mol.Atoms.MolecularWeight(), 1);
	}

	[Fact]
	public void Molecule_CanBeCreatedEmpty()
	{
		var mol = new Molecule();
		Assert.Empty(mol.Atoms);
		Assert.Empty(mol.Bonds);
	}

	[Fact]
	public void Molecule_AtomsAndBondsCanBeAdded()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("He", 0, 0, 1));
		mol.Atoms.Add(new Atom("He", 0, 0, 0));
		mol.Bonds.Add(new Bond(mol.Atoms[0], mol.Atoms[1]));
		Assert.Equal(2, mol.Atoms.Count);
		Assert.Single(mol.Bonds);
		Assert.Equal("He2", mol.Atoms.SumFormula());
	}

	[Fact]
	public void Molecule_ElectronCountIsSumOfAtoms()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("He", 0, 0, 1));
		mol.Atoms.Add(new Atom("He", 0, 0, 0));
		Assert.Equal(4, mol.Electrons);
	}

	[Fact]
	public void Molecule_ElectronsAreSummedWithAtomCharges()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("He", 0, 0, 1));
		mol.Atoms.Add(new Atom("He", 0, 0, 0) {Charge = 1});
		Assert.Equal(3, mol.Electrons);
	}

	[Fact]
	public void Molecule_ShouldBehaveCorrectToCharge()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("He", 0, 0, 1));
		mol.Atoms.Add(new Atom("He", 0, 0, 0) {Charge = 1});
		mol.ImplicitCharge = 1;
		Assert.Equal(2, mol.Electrons);
		Assert.Equal(2, mol.Charge);
	}

	[Fact]
	public void Molecule_ShouldHaveMultiplicityAndSpin()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("Mo"));
		for (var i = 0; i < 6; i++)
		{
			mol.Atoms.Add(new Atom("C"));
			mol.Atoms.Add(new Atom("O"));
		}

		Assert.Equal(1, mol.Multiplicity);
		Assert.Equal(0, mol.Spin);
		Assert.False(mol.IsParamagnetic);
	}

	[Fact]
	public void Molecule_ShouldBeParamagnetic_IfElectronNumberIsOdd()
	{
		var mol = new Molecule();
		mol.Atoms.Add(new Atom("Sc"));
		mol.Multiplicity = 2;
		Assert.Equal(.5, mol.Spin);
		Assert.Equal(2, mol.Multiplicity);
		Assert.True(mol.IsParamagnetic);
	}
}

public class MoleculeTestDataGenerator : IEnumerable<object[]>
{
	//creates a list with objects containing path, atoms count, bonds count
	//sum formula and molecular weight#
	//chemspider 2D files (.mol) do not add implicit hydrogens
	private readonly List<object[]> _data = new()
	{
		new object[] {"files/cif.cif", 79, 89, "C40Cl2H29MoN4O3", 780.51},
		new object[] {"files/cif_noTrim.cif", 79, 89, "C40Cl2H29MoN4O3", 780.51},
		new object[] {"files/benzene.mol2", 12, 12, "C6H6", 78.11184},
		new object[] {"files/myo.mol2", 1437, 1312, null!, null!},
		new object[] {"files/2spl.pdb", 1437, 1314, null!, null!},
		new object[] {"files/1hv4.pdb", 9288, 9562, null!, null!},
		new object[] {"files/oriluy.pdb", 130, 151, null!, null!},
		new object[] {"files/mescho.xyz", 23, 23, "C10H12O", 148.205},
		new object[] {"files/tep.mol", 46, 50, "C28H14N4", null!},
		new object[] {"files/benzene.mol", 6, 6, "C6", 72.1},
		new object[] {"files/benzene_arom.mol", 12, 12, "C6H6", 78.11184},
		new object[] {"files/corrole.mol", 37, 41, null!, null!}
	};

	public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
