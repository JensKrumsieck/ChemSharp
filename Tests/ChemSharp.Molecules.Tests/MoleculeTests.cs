using Xunit;

namespace ChemSharp.Molecules.Tests;

public class MoleculeTests
{
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
		Assert.Equal("He2", mol.SumFormula);
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

	[Fact]
	public void Molecule_ShouldRecalculateBonds()
	{
		const string file = "files/cif.cif";
		var mol = Molecule.FromFile(file);
		mol.RecalculateBonds();
		Assert.Equal(89, mol.Bonds.Count);
	}
}
