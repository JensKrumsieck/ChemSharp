using BenchmarkDotNet.Attributes;
using ChemSharp.Extensions;
using ChemSharp.GraphTheory;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser]
[MarkdownExporter]
public class ConnectedFiguresBenchmarks
{
	private readonly UndirectedGraph<int, Edge<int>> _graph;
	public ConnectedFiguresBenchmarks()
	{
		var vertices = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
		var edges = new[]
		{
			new Edge<int>(1, 3), new Edge<int>(2, 4), new Edge<int>(3, 5), new Edge<int>(3, 6),
			new Edge<int>(4, 7), new Edge<int>(5, 8), new Edge<int>(5, 6), new Edge<int>(8, 9),
			new Edge<int>(9, 10),
		};
		_graph = new UndirectedGraph<int, Edge<int>>(vertices, edges);
	}
	[Benchmark(Baseline = true)]
	public void OldConnectedFigures()
	{
		var result =DFSUtil.ConnectedFigures(_graph.Vertices, v => _graph.Neighbors(v));
	}

	[Benchmark]
	public void NewConnectedFigures()
	{
		var result = _graph.ConnectedFigures();
	}
}
