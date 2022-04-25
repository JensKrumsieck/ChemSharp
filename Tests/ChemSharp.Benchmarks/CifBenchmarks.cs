using BenchmarkDotNet.Attributes;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Formats;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser, MarkdownExporter]
public class CifBenchmarks
{
	[Params("files/cif.cif")] public string CifFile { get; set; }

	[Benchmark]
	public void DataProviderMethodMol2()
	{
		var mol = new Molecule(new CIFDataProvider(CifFile));
	}

	[Benchmark(Baseline = true)]
	public void FormatMethodMol2()
	{
		var mol = CifFormat.Read(CifFile);
	}
}
