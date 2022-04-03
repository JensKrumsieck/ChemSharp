using BenchmarkDotNet.Attributes;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Formats;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser]
[MarkdownExporter]
public class PdbBenchmarks
{
	[Params("files/oriluy.pdb", "files/1hv4.pdb")]
	public string file { get; set; }

	[Benchmark(Baseline = true)]
	public void DataProviderMethod()
	{
		var mol = new Molecule(new PDBDataProvider(file));
	}

	[Benchmark]
	public void FormatMethod()
	{
		var mol = PDBFormat.Read(file);
	}
}
