using ChemSharp.Export;
using ChemSharp.Molecules;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ChemSharp.Rendering.Svg;

namespace ChemSharp.Rendering.Export
{
    /// <summary>
    /// Exports Molecule to Svg
    /// </summary>
    public class SvgExporter : IExporter
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public static void Export(Molecule mol, string filename, double width, double height)
        {
            var svgMol = mol.ToSvg();
            var exporter = new SvgExporter { Width = width, Height = height };
            exporter.Export(svgMol, File.Create(filename));
        }

        /// <summary>
        /// Exports SvgMolecule To String
        /// </summary>
        /// <param name="mol"></param>
        public string ExportToString(SvgMolecule mol)
        {
            //add namespaces
            var ns = new XmlSerializerNamespaces();
            ns.Add("xlink", "http://www.w3.org/1999/xlink");

            //add svg tags
            var doc = new SvgRoot { ActualWidth = Width, ActualHeight = Height };
            doc.Elements.AddRange(mol.Tags);

            //create serializer and writer objects
            var ser = new XmlSerializer(typeof(SvgRoot));
            using var sw = new UTF8Writer();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                Indent = true
            });
            //write doc type
            xw.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            ser.Serialize(xw, doc, ns);
            return sw.ToString();
        }

        /// <summary>
        /// Exports to SVG File
        /// </summary>
        /// <param name="exportable"></param>
        /// <param name="stream"></param>
        public void Export(IExportable exportable, Stream stream)
        {
            if (!(exportable is SvgMolecule mol)) throw new NotSupportedException("Please use SvgMolecule Type");

            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(ExportToString(mol));
        }
    }
}
