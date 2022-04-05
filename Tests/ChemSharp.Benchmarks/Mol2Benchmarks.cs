using BenchmarkDotNet.Attributes;
using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Formats;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser, MarkdownExporter]
public class Mol2Benchmarks
{
	[Params("files/myo.mol2", "files/benzene.mol2")]
	public string mol2File { get; set; }

	[Benchmark]
	public void DataProviderMethodMol2()
	{
		var mol = new Molecule(new Mol2DataProvider(mol2File));
	}

	[Benchmark(Baseline = true)]
	public void FormatMethodMol2()
	{
		var mol = Mol2Format.Read(mol2File);
	}
}
