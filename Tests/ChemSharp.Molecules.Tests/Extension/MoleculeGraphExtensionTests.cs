using System.Diagnostics;
using ChemSharp.Molecules.Extensions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Extension;

public class MoleculeGraphExtensionTests
{
	private readonly Molecule _mol;

	public MoleculeGraphExtensionTests()
	{
		//WARNING! Do NOT sort!
		_mol = new Molecule();
		_mol.Atoms.Add(new Atom("C", 1.2130f, .6880f, 0f));
		_mol.Atoms.Add(new Atom("C", 1.203f, -.706f, 0f));
		_mol.Atoms.Add(new Atom("C", -.01f, -1.395f, 0f));
		_mol.Atoms.Add(new Atom("C", -1.2130f, -.6880f, 0f));
		_mol.Atoms.Add(new Atom("C", -1.2130f, .706f, 0f));
		_mol.Atoms.Add(new Atom("C", .01f, 1.395f, 0f));
		_mol.RecalculateBonds();
		Debug.Assert(_mol.Bonds.Count == 6);
	}

	[Fact]
	public void AdjacencyMatrix_ShouldGenerate()
	{
		var adjacency = _mol.AdjacencyMatrix();
		Assert.Equal(adjacency.Length, _mol.Atoms.Count);
		Assert.True(adjacency.IsSymmetric());
	}

	[Fact]
	public void AdjacencyMatrix_ShouldBeValid()
	{
		var adjacency = _mol.AdjacencyMatrix();
		var result = new MatrixInt(new[,]
		{
			//0 1 2 3 4 5
			{0, 1, 0, 0, 0, 1}, //0
			{1, 0, 1, 0, 0, 0}, //1
			{0, 1, 0, 1, 0, 0}, //2
			{0, 0, 1, 0, 1, 0}, //3
			{0, 0, 0, 1, 0, 1}, //4
			{1, 0, 0, 0, 1, 0}, //5
		});
		Assert.Equal(result, adjacency);
	}

	[Fact]
	public void DistanceMatrix_CanBeCreated()
	{
		var distance = _mol.DistanceMatrix();
		Assert.Equal(distance.Length, _mol.Atoms.Count);
		Assert.True(distance.IsSymmetric());
	}
}
