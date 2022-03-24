using System.Diagnostics;
using System.IO;
using ChemSharp.Export;
using ChemSharp.Extensions;

namespace ChemSharp.Molecules.Export;

/// <summary>
///     Exports a Molecule to XYZ
/// </summary>
public class XYZExporter : AbstractMoleculeExporter
{
	public static void Export(Molecule molecule, string filename)
	{
		var exporter = new XYZExporter();
		using var stream = File.Create(filename);
		exporter.Export(molecule, stream);
	}

	public override void Export(IExportable exportable, Stream stream)
	{
		base.Export(exportable, stream);
		Debug.Assert(Molecule != null, nameof(Molecule) + " != null");

		var count = Molecule.Atoms.Count;
		var name = Molecule.Title;
		using var sw = new StreamWriter(stream);
		sw.WriteLine(count);
		sw.WriteLine(name);
		foreach (var atom in Molecule.Atoms)
			sw.WriteLine($"{atom.Symbol}" +
			             $"\t{atom.Location.X.ToInvariantString()}" +
			             $"\t{atom.Location.Y.ToInvariantString()}" +
			             $"\t{atom.Location.Z.ToInvariantString()}");
	}
}
