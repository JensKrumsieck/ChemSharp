using ChemSharp.Export;
using ChemSharp.Molecules;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Rendering.Export
{
    public class PovRayExporter : IExporter
    {
        public Camera Cam { get; set; }
        public List<Light> Lights { get; set; } = new();

        /// <summary>
        /// Export Molecule to .pov file
        /// </summary>
        /// <param name="mol"></param>
        /// <param name="filename"></param>
        /// <param name="cam"></param>
        /// <param name="lights"></param>
        public static void Export(Molecule mol, string filename, Camera cam, List<Light> lights)
        {
            var exporter = new PovRayExporter { Cam = cam, Lights = lights };
            exporter.Export(mol, File.Create(filename));
        }

        /// <summary>
        /// Exports Molecule to PovRay format string
        /// </summary>
        /// <param name="mol"></param>
        /// <returns></returns>
        public string ExportToString(Molecule mol)
        {
            if (Cam == null) throw new NotSupportedException("Camera not set!");
            if (Lights.Count == 0) throw new NotSupportedException("No Lights found!");
            var atoms = mol.Atoms.Select(a =>
                    new Sphere
                    {
                        Location = a.Location,
                        Radius = (a.CovalentRadius ?? 100) / 200f,
                        Color = a.Color.HexColorToVector()
                    }
                        .ToPovString())
                .ToList();
            var bonds = mol.Bonds.Select(b =>
                new Cylinder
                {
                    Start = b.Atom1.Location,
                    End = b.Atom2.Location,
                    Radius = .1f,
                    Color = "#cccccc".HexColorToVector()
                }
                    .ToPovString())
                .ToList();
            var atomString = string.Join(Environment.NewLine, atoms);
            var bondString = string.Join(Environment.NewLine, bonds);
            var lightString = string.Join(Environment.NewLine, Lights.Select(s => s.ToPovString()));
            return string.Join(Environment.NewLine, Cam.ToPovString(), lightString, atomString, bondString);
        }


        /// <inheritdoc />
        public void Export(IExportable exportable, Stream stream)
        {
            if (exportable is not Molecule m) throw new NotSupportedException();
            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(ExportToString(m));
        }
    }
}
