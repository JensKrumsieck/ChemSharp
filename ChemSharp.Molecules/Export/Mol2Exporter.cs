using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ChemSharp.Export;
using ChemSharp.Extensions;

namespace ChemSharp.Molecules.Export;

public class Mol2Exporter : AbstractMoleculeExporter
{
    private readonly Dictionary<Atom, int> _atomsDictionary = new();

    public static void Export(Molecule mol, string filename)
    {
        var exporter = new Mol2Exporter();
        using var stream = File.Create(filename);
        exporter.Export(mol, stream);
    }

    /// <summary>
    /// Exports molecule to mol2 file
    /// </summary>
    /// <param name="exportable"></param>
    /// <param name="stream"></param>
    public override void Export(IExportable exportable, Stream stream)
    {
        base.Export(exportable, stream);
        Debug.Assert(Molecule != null, nameof(Molecule) + " != null");
        var atomsCount = Molecule.Atoms.Count;
        var bondsCount = Molecule.Bonds.Count;
        using var sw = new StreamWriter(stream);
        sw.WriteLine($"@<TRIPOS>MOLECULE\n{Molecule.Title}");
        sw.WriteLine($"{atomsCount} {bondsCount} {0} {0} {0}\n");
        sw.WriteLine("SMALL\nNO_CHARGES\n\n@<TRIPOS>ATOM");
        for (var i = 0; i < atomsCount; i++)
        {
            var atom = Molecule.Atoms[i];
            _atomsDictionary.Add(atom, i + 1);
            sw.WriteLine(AtomBlock(atom, i));
        }
        sw.WriteLine("@<TRIPOS>BOND");
        for (var i = 0; i < bondsCount; i++)
        {
            var bond = Molecule.Bonds[i];
            sw.WriteLine(BondBlock(bond, i));
        }
    }

    private static string AtomBlock(Atom atom, int i) => $"{StringUtil.Spaces(i < 9 ? 5 : 4)}{i + 1} {atom.Title}" +
                                                         $"{StringUtil.Spaces(4)}" +
                                                         $"{CoordinateString(atom.Location.X)}" +
                                                         $"{CoordinateString(atom.Location.Y)}" +
                                                         $"{CoordinateString(atom.Location.Z)}" +
                                                         $" {atom.Symbol}" +
                                                         $"{StringUtil.Spaces(4)}1" +
                                                         $" UNL1 {StringUtil.Spaces(4)} 0.000";
    private static string CoordinateString(float position) => $"{StringUtil.Spaces(position > 0 && position < 10 ? 4 : 3)}{position.ToString("N4", CultureInfo.InvariantCulture)}";
    private string BondBlock(Bond bond, int i)
    {
        var atom1 = _atomsDictionary[bond.Atom1];
        var atom2 = _atomsDictionary[bond.Atom2];
        return $"{StringUtil.Spaces(i < 9 ? 5 : 4)}{i + 1}" +
               $"{StringUtil.Spaces(atom1 < 10 ? 5 : 4)}{atom1}" +
               $"{StringUtil.Spaces(atom2 < 10 ? 5 : 4)}{atom2}" +
               $"{StringUtil.Spaces(bond.IsAromatic ? 4 : 5)}{(bond.IsAromatic ? "ar" : bond.Order.ToString())}";
    }
}
