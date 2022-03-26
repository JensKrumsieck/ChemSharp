using System.Collections.Generic;
using System.Numerics;
using ChemSharp.GraphTheory;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Base.Tests.GraphTheory;

public class GraphTests
{
	[Fact]
	public void Graph_ShouldBeInitialized()
	{
		var graph = new UndirectedGraph<Vector3, Edge<Vector3>>();
		var v1 = new Vector3(1, 2, 3);
		var v2 = new Vector3(2, 2, 1);
		var v3 = new Vector3(2, 2, 0);
		graph.Vertices.UnionWith(new[] {v1, v2, v3});
		graph.Edges.Add(new Edge<Vector3>(v1, v2));
		graph.Edges.Add(new Edge<Vector3>(v1, v3));
		var edges = graph.Edges;
		var vertices = graph.Vertices;
		edges.Count.Should().Be(2);
		vertices.Count.Should().Be(3);
	}

	private UndirectedGraph<string, Edge<string>> _graph;
	public GraphTests() =>
		_graph = new UndirectedGraph<string, Edge<string>>(
			new[] {"Hallo", "Welt", "Ihr", "Lieben"},
			new[]
			{
				new Edge<string>("Hallo", "Ihr"), new Edge<string>("Ihr", "Lieben"),
				new Edge<string>("Hallo", "Welt")
			}
		);

	[Fact]
	public void Graph_AutoRecalculateCache()
	{
		_graph.Neighbors("Hallo").Count.Should().Be(2);
		//recalculates cache automatically
		_graph.Vertices.Add("Du");
		_graph.Edges.Add(new Edge<string>("Hallo", "Du"));
		_graph.Neighbors("Hallo").Count.Should().Be(3);
		_graph.Neighbors("Hallo").Should().BeEquivalentTo(new List<string> {"Du", "Welt", "Ihr"});
	}

	[Fact]
	public void Graph_NeighborsNoCache()
	{
		_graph.Neighbors("Hallo").Count.Should().Be(2);
		_graph.CacheNeighbors = false;
		_graph.Vertices.Add("Du");
		_graph.Edges.Add(new Edge<string>("Hallo", "Du"));
		_graph.Neighbors("Hallo").Count.Should().Be(3);
		_graph.Neighbors("Hallo").Should().BeEquivalentTo(new List<string> {"Du", "Welt", "Ihr"});
	}
}
