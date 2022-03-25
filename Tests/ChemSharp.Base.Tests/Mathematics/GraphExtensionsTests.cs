using Xunit;
using static ChemSharp.Mathematics.GraphExtensions;

namespace ChemSharp.Base.Tests.Mathematics;

public class GraphExtensionsTests
{
	[Fact]
	public void Dijkstra_ShouldBeValid()
	{
		//test values from https://www.geeksforgeeks.org/csharp-program-for-dijkstras-shortest-path-algorithm-greedy-algo-7/
		var graph = new[,]
		{
			{0, 4, 0, 0, 0, 0, 0, 8, 0}, {4, 0, 8, 0, 0, 0, 0, 11, 0}, {0, 8, 0, 7, 0, 4, 0, 0, 2},
			{0, 0, 7, 0, 9, 14, 0, 0, 0}, {0, 0, 0, 9, 0, 10, 0, 0, 0}, {0, 0, 4, 14, 10, 0, 2, 0, 0},
			{0, 0, 0, 0, 0, 2, 0, 1, 6}, {8, 11, 0, 0, 0, 0, 1, 0, 7}, {0, 0, 2, 0, 0, 0, 6, 7, 0}
		};
		var expected = new[] {0, 4, 12, 19, 21, 11, 9, 8, 14};
		var result = Dijkstra(graph, 0);
		Assert.Equal(expected, result);
	}
}
