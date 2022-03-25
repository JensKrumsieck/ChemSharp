namespace ChemSharp.Mathematics;

public static class GraphExtensions
{
	/// <summary>
	///     Returns the Distance Matrix using Dijkstra Algorithm
	/// </summary>
	/// <param name="adjacency"></param>
	/// <returns></returns>
	public static MatrixInt DistanceMatrix(this MatrixInt adjacency)
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
	///     Implementation of Dijkstra Algorithm
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
				if (!shortest[v] && adjacency[j, v] != 0 && dist[j] != int.MaxValue &&
				    dist[j] + adjacency[j, v] < dist[v])
					dist[v] = dist[j] + adjacency[j, v];
		}

		return dist;
	}

	/// <summary>
	///     Get Minimum Distance between to vertices
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
}
