using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemSharp.Extensions;

/// <summary>
///     A static class for operations on undirected graphs
/// </summary>
public static class DFSUtil
{
	/// <summary>
	///     List all connected figures
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="func"></param>
	/// <returns></returns>
	public static IEnumerable<IEnumerable<T>> ConnectedFigures<T>(IEnumerable<T> list, Func<T, IEnumerable<T>> func)
	{
		var visited = new HashSet<T>();
		foreach (var item in list)
			if (!visited.Contains(item))
				yield return DepthFirstSearch(item, func, visited);
	}

	/// <summary>
	///     DFS Algorithm
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="vertex"></param>
	/// <param name="func"></param>
	/// <param name="visited"></param>
	/// <returns></returns>
	public static HashSet<T> DepthFirstSearch<T>(T vertex, Func<T, IEnumerable<T>> func, HashSet<T> visited = null)
	{
		var results = new HashSet<T>();
		var stack = new Stack<T>();
		stack.Push(vertex);
		while (stack.Count > 0)
		{
			var current = stack.Pop();
			if (results.Contains(current)) continue;

			results.Add(current);
			foreach (var n in func(current))
				if (!results.Contains(n))
					stack.Push(n);
		}

		visited?.UnionWith(results);
		return results;
	}

	/// <summary>
	///     Implementation of recursive DFS
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="vertex"></param>
	/// <param name="func"></param>
	/// <param name="visited">Provide a Hashset if you need to track which figure already has been found</param>
	/// <returns></returns>
	[Obsolete("Use DepthFirstSearch() instead")]
	public static async Task<HashSet<T>> DFS<T>(T vertex, Func<T, IEnumerable<T>> func, HashSet<T> visited)
	{
		var results = new HashSet<T>();
		await Traverse(vertex, results, func);

		//add results to visited if exists
		visited?.UnionWith(results);
		return visited;
	}

	/// <summary>
	///     Traverse Function
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="vertex"></param>
	/// <param name="visited"></param>
	/// <param name="func"></param>
	[Obsolete("Use DepthFirstSearch() instead")]
	private static async Task Traverse<T>(T vertex, ISet<T> visited, Func<T, IEnumerable<T>> func)
	{
		visited.Add(vertex);
		foreach (var neighbor in func(vertex))
			if (!visited.Contains(neighbor))
				await Traverse(neighbor, visited, func);
	}

	/// <summary>
	///     Calculates the vertex degree (count) of vertex by func(vertex)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="vertex"></param>
	/// <param name="func"></param>
	/// <returns></returns>
	public static int VertexDegree<T>(T vertex, Func<T, IEnumerable<T>> func) => func(vertex).Count();

	/// <summary>
	///     Uses backtracking for path detection
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="start"></param>
	/// <param name="goal"></param>
	/// <param name="neighbors"></param>
	/// <param name="limit"></param>
	/// <returns></returns>
	public static HashSet<T> BackTrack<T>(T start, T goal, Func<T, IEnumerable<T>> neighbors, int limit)
	{
		var path = new HashSet<T> {start};
		var maxSteps = 2000;
		BackTrack(goal, path, neighbors, limit, ref maxSteps);
		return path;
	}

	/// <summary>
	///     Recursive Implementation of backtracking
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="goal"></param>
	/// <param name="visited"></param>
	/// <param name="neighbors"></param>
	/// <param name="limit"></param>
	/// <param name="maxSteps"></param>
	/// <returns></returns>
	private static bool BackTrack<T>(T goal, ISet<T> visited, Func<T, IEnumerable<T>> neighbors, int limit,
	                                 ref int maxSteps)
	{
		if (--maxSteps <= 0) return false;

		var last = visited.Last();
		if (Equals(last, goal) && visited.Count == limit)
		{
			visited.Add(goal);
			return true;
		}

		if (visited.Count >= limit) return false;

		foreach (var neighbor in neighbors(last))
		{
			if (visited.Contains(neighbor)) continue;

			visited.Add(neighbor);
			if (BackTrack(goal, visited, neighbors, limit, ref maxSteps)) return true;

			visited.Remove(visited.Last());
		}

		return false;
	}
}
