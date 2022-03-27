using BenchmarkDotNet.Attributes;
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
		var prov = new PDBDataProvider(file);
		prov.ReadData();
		var atoms = prov.Atoms.ToList();
		//molecule would also allocate a list
	}

	[Benchmark]
	public void FormatMethod()
	{
		var atoms = PDBFormat.Read(file);
	}
}
