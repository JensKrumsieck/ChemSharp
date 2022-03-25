using ChemSharp.Mathematics;

namespace ChemSharp.Molecules.Extensions;

public static class MoleculeGraphExtension
{
	/// <summary>
	/// Returns a adjacency matrix where 1 means bond and 0 means no bond
	/// </summary>
	/// <param name="mol"></param>
	/// <returns></returns>
	public static MatrixInt AdjacencyMatrix(this Molecule mol)
	{
		var adjacent = new MatrixInt(mol.Atoms.Count);
		for (var i = 0; i < mol.Atoms.Count; i++)
		{
			for (var j = 0; j < mol.Atoms.Count; j++)
			{
				adjacent[i, j] = mol.Neighbors(mol.Atoms[i]).Contains(mol.Atoms[j]) && i != j ? 1 : 0;
			}
		}

		return adjacent;
	}

	public static MatrixInt DistanceMatrix(this Molecule mol) => mol.AdjacencyMatrix().DistanceMatrix();
}
