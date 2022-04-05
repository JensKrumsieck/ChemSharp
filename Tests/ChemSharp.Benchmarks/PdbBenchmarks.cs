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
	public string pdbFile { get; set; }

	[Benchmark]
	public void DataProviderMethodPdb()
	{
		var mol = new Molecule(new PDBDataProvider(pdbFile));
	}

	[Benchmark(Baseline = true)]
	public void FormatMethodPdb()
	{
		var mol = PDBFormat.Read(pdbFile);
	}
}
