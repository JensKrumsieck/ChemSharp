using ChemSharp.Export;
using ChemSharp.Extensions;

namespace ChemSharp.Molecules.Export;

public class PDBExporter : AbstractMoleculeExporter
{
	public static void Export(Molecule mol, string filename)
	{
		var exporter = new PDBExporter();
		using var stream = File.Create(filename);
		exporter.Export(mol, stream);
	}

	public override void Export(IExportable exportable, Stream stream)
	{
		base.Export(exportable, stream);
		if (Molecule == null) return;
		var atomsCount = Molecule.Atoms.Count;
		using var sw = new StreamWriter(stream);
		sw.WriteLine("HEADER");
		sw.WriteLine($"TITLE{StringUtil.Spaces(5)}{Molecule.Title}");

		var line = new char[80];
		for (var i = 0; i < atomsCount; i++)
		{
			var atom = Molecule.Atoms[i];
			Array.Fill(line, ' ');
			AppendToCharArrayAt(in line, "ATOM", 0, PdbAlign.Left);
			AppendToCharArrayAt(in line, i.ToString(), 11);
			AppendToCharArrayAt(in line, atom.Title, 13, PdbAlign.Left);
			AppendToCharArrayAt(in line, atom.Residue, 20);
			AppendToCharArrayAt(in line, atom.ChainId != default ? atom.ChainId : ' ', 22);
			AppendToCharArrayAt(in line, atom.ResidueId.ToString(), 26);
			AppendToCharArrayAt(in line, atom.Location.X.ToInvariantString(), 38);
			AppendToCharArrayAt(in line, atom.Location.Y.ToInvariantString(), 46);
			AppendToCharArrayAt(in line, atom.Location.Z.ToInvariantString(), 54);
			AppendToCharArrayAt(in line, atom.Symbol, 78);
			AppendToCharArrayAt(in line, atom.Charge.ToString(), 80);
			sw.WriteLine(line);
		}
	}

	private static void AppendToCharArrayAt(in char[] arr, char chr, int index) => arr[index] = chr;

	//if align is right, give end column index
	private static void AppendToCharArrayAt(in char[] arr, ReadOnlySpan<char> str, int index,
		PdbAlign align = PdbAlign.Right)
	{
		switch (align)
		{
			case PdbAlign.Left:
			{
				for (var i = 0; i < str.Length; i++) arr[index + i] = str[i];
				break;
			}
			case PdbAlign.Right:
			{
				for (var i = 0; i < str.Length; i++) arr[index - str.Length + i] = str[i];
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(align), align, null);
		}
	}

	private enum PdbAlign
	{
		Left,
		Right
	}
}
