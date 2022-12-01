using BenchmarkDotNet.Attributes;
using ChemSharp.Extensions;
using ChemSharp.GraphTheory;
using ChemSharp.Molecules;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser]
public class ConnectedFiguresMoleculeBenchmarks
{
	private readonly Molecule _graph = MoleculeFactory.Create("files/cif.cif");

	[Benchmark(Baseline = true)]
	public void OldConnectedFigures()
	{
		var result = DFSUtil.ConnectedFigures(_graph.Vertices, v => _graph.Neighbors(v));
	}

	[Benchmark]
	public void NewConnectedFigures()
	{
		var result = _graph.ConnectedFigures();
	}
}
