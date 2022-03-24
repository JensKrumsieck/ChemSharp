using System.Diagnostics;
using ChemSharp.Molecules.Extensions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Extension;

public class MoleculeGraphExtensionTests
{
	private Molecule mol;

	public MoleculeGraphExtensionTests()
	{
		//WARNING! Do NOT sort!
		mol = new Molecule();
		mol.Atoms.Add(new Atom("C", 1.2130f, .6880f, 0f));
		mol.Atoms.Add(new Atom("C", 1.203f, -.706f, 0f));
		mol.Atoms.Add(new Atom("C", -.01f, -1.395f, 0f));
		mol.Atoms.Add(new Atom("C", -1.2130f, -.6880f, 0f));
		mol.Atoms.Add(new Atom("C", -1.2130f, .706f, 0f));
		mol.Atoms.Add(new Atom("C", .01f, 1.395f, 0f));
		mol.RecalculateBonds();
		Debug.Assert(mol.Bonds.Count == 6);
	}

	[Fact]
	public void AdjacencyMatrix_ShouldGenerate()
	{
		var adjacency = mol.AdjacencyMatrix();
		Assert.Equal(adjacency.Length, mol.Atoms.Count);
		Assert.True(MoleculeGraphExtension.IsSymmetric(adjacency));
	}

	[Fact]
	public void AdjacencyMatrix_ShouldBeValid()
	{
		var adjacency = mol.AdjacencyMatrix();
		var result = new int[,]
		{
			//0 1 2 3 4 5
			{0, 1, 0, 0, 0, 1}, //0
			{1, 0, 1, 0, 0, 0}, //1
			{0, 1, 0, 1, 0, 0}, //2
			{0, 0, 1, 0, 1, 0}, //3
			{0, 0, 0, 1, 0, 1}, //4
			{1, 0, 0, 0, 1, 0}, //5
		};
		Assert.Equal(result, adjacency);
	}

	[Fact]
	public void DistanceMatrix_CanBeCreated()
	{
		var distance = mol.DistanceMatrix();
		Assert.Equal(distance.Length, mol.Atoms.Count);
		Debug.Write(distance);
	}
}
