﻿using ChemSharp.Export;
using System;
using System.Globalization;
using System.IO;

namespace ChemSharp.Molecules.Export
{
    /// <summary>
    /// Exports a Molecule to XYZ
    /// </summary>
    public class XYZExporter : IExporter
    {
        public static void Export(Molecule molecule, string filename)
        {
            var exporter = new XYZExporter();
            using var stream = File.Create(filename);
            exporter.Export(molecule, stream);
        }

        public void Export(IExportable exportable, Stream stream)
        {
            if (!(exportable is Molecule mol)) throw new NotSupportedException("Please use Molecule Type");
            var count = mol.Atoms.Count;
            var name = mol.Title;
            using var sw = new StreamWriter(stream);
            sw.WriteLine(count);
            sw.WriteLine(name);
            foreach(var atom in mol.Atoms) 
                sw.WriteLine($"{atom.Symbol}" +
                             $"\t{atom.Location.X.ToString(CultureInfo.InvariantCulture)}" +
                             $"\t{atom.Location.Y.ToString(CultureInfo.InvariantCulture)}" +
                             $"\t{atom.Location.Z.ToString(CultureInfo.InvariantCulture)}");
            sw.Close();
        }
    }
}
