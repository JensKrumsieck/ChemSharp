using System.Linq;
using ChemSharp.GraphTheory;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class MoleculeGraphTheoryTests
{
	[Theory]
	[InlineData("files/cif.cif")]
	[InlineData("files/benzene_arom.mol")]
	[InlineData("files/mescho.xyz")]
	[InlineData("files/corrole.mol")]
	public void Molecule_DFSIsValid(string path)
	{
		var mol = Molecule.FromFile(path);
		var dfs = mol.DepthFirstSearch();
		//every atom is detected
		dfs.Count.Should().Be(mol.Atoms.Count);
	}

	[Theory]
	[InlineData("files/oriluy.pdb", 27)]
	[InlineData("files/myo.mol2", 209)]
	public void Molecule_DFSFindsOnlyConnectedAtoms(string path, int less)
	{
		var mol = Molecule.FromFile(path);
		var dfs = mol.DepthFirstSearch();
		//every atom is detected
		dfs.Count.Should().NotBe(mol.Atoms.Count);
		dfs.Count.Should().Be(mol.Atoms.Count - less);
	}

	[Theory]
	[InlineData("files/oriluy.pdb", 27)]
	[InlineData("files/myo.mol2", 209)]
	public void Molecule_BFSFindsOnlyConnectedAtoms(string path, int less)
	{
		var mol = Molecule.FromFile(path);
		var bfs = mol.BreadthFirstSearch();
		//every atom is detected
		bfs.Count.Should().NotBe(mol.Atoms.Count);
		bfs.Count.Should().Be(mol.Atoms.Count - less);
	}

	[Theory]
	[InlineData("files/cif.cif")]
	[InlineData("files/benzene_arom.mol")]
	[InlineData("files/mescho.xyz")]
	[InlineData("files/corrole.mol")]
	public void Molecule_BFSIsValid(string path)
	{
		var mol = Molecule.FromFile(path);
		var bfs = mol.BreadthFirstSearch();
		//every atom is detected
		bfs.Count.Should().Be(mol.Atoms.Count);
	}

	[Theory]
	[InlineData("files/cif.cif", 1)]
	[InlineData("files/oriluy.pdb", 3)]
	[InlineData("files/corrole.mol", 1)]
	public void Molecule_ConnectedFiguresIsValid(string path, int figures)
	{
		var mol = Molecule.FromFile(path);
		var cf = mol.ConnectedFigures();
		cf.Count().Should().Be(figures);
	}
}
