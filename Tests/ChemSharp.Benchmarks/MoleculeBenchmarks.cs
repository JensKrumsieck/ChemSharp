using BenchmarkDotNet.Attributes;
using ChemSharp.Molecules;

namespace ChemSharp.Benchmarks;

[MemoryDiagnoser, MarkdownExporter]
public class MoleculeBenchmarks
{
	[Params("files/myo.mol2", "files/benzene.mol2", "files/1hv4.pdb", "files/cif.cif", "files/oriluy.pdb")]
	public string molFile { get; set; }

	[Benchmark]
	public void DataProviderMethod()
	{
		var mol = MoleculeFactory.Create(molFile);
	}

	[Benchmark(Baseline = true)]
	public void FormatMethod()
	{
		var mol = Molecule.FromFile(molFile);
	}
}
