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

	public static MatrixInt DistanceMatrix(this Molecule mol) => DistanceMatrix(mol.AdjacencyMatrix());

	/// <summary>
	/// Returns the Distance Matrix using Dijkstra Algorithm
	/// </summary>
	/// <param name="adjacency"></param>
	/// <returns></returns>
	public static MatrixInt DistanceMatrix(MatrixInt adjacency)
	{
		var len = adjacency.Length;
		var dist = new MatrixInt(len);
		for (var i = 0; i < len; i++)
		{
			var path = Dijkstra(adjacency, i);
			for (var j = 0; j < path.Length; j++)
				dist[i, j] = path[j];
		}

		return dist;
	}

	/// <summary>
	/// Implementation of Dijkstra Algorithm
	/// </summary>
	/// <param name="adjacency">Adjacency Matrix</param>
	/// <param name="srcV">Source Vertex</param>
	/// <returns></returns>
	public static int[] Dijkstra(MatrixInt adjacency, int srcV)
	{
		var len = adjacency.Length;
		var dist = new int[len];
		var shortest = new bool[len];
		for (var i = 0; i < len; i++)
		{
			dist[i] = int.MaxValue;
			shortest[i] = false;
		}

		dist[srcV] = 0;
		for (var i = 0; i < len - 1; i++)
		{
			var j = MinimumDistance(dist, shortest);
			shortest[j] = true;
			for (var v = 0; v < len; v++)
			{
				if (!shortest[v] && adjacency[j, v] != 0 && dist[j] != int.MaxValue &&
				    dist[j] + adjacency[j, v] < dist[v])
					dist[v] = dist[j] + adjacency[j, v];
			}
		}

		return dist;
	}

	/// <summary>
	/// Get Minimum Distance between to vertices
	/// </summary>
	/// <param name="dist"></param>
	/// <param name="shortest"></param>
	/// <returns></returns>
	private static int MinimumDistance(int[] dist, bool[] shortest)
	{
		var min = int.MaxValue;
		var iMin = -1;
		for (var i = 0; i < shortest.Length; i++)
		{
			if (shortest[i] || dist[i] > min) continue;
			min = dist[i];
			iMin = i;
		}

		return iMin;
	}

	/// <summary>
	/// Transposes Matrix
	/// </summary>
	/// <param name="src"></param>
	/// <returns></returns>
	public static int[,] Transpose(MatrixInt src)
	{
		var len = src.Length;
		var transposed = new MatrixInt(len);
		for (var i = 0; i < len; i++)
		{
			for (var j = 0; j < len; j++)
			{
				transposed[j, i] = src[i, j];
			}
		}

		return transposed;
	}

	/// <summary>
	/// Indicates whether Matrix is Symmetric
	/// </summary>
	/// <param name="src"></param>
	/// <returns></returns>
	public static bool IsSymmetric(int[,] src) => src == Transpose(src);
}
