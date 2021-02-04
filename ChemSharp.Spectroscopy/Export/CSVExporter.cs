using ChemSharp.Export;
using ChemSharp.Extensions;
using ChemSharp.Spectroscopy.Extension;
using System.IO;

namespace ChemSharp.Spectroscopy.Export
{
    public class CSVExporter : AbstractSpectrumExporter
    {

        /// <summary>
        /// CSV Separator
        /// </summary>
        public char Separator = ',';

        public static void Export(Spectrum spc, string filename, char separator, SpectrumExportFlags flags = SpectrumExportFlags.Experimental)
        {
            var exporter = new CSVExporter { Separator = separator, Flags = flags };
            using var stream = File.Create(filename);
            exporter.Export(spc, stream);
        }

        public static string ExportToString(Spectrum spc, char separator, SpectrumExportFlags flags = SpectrumExportFlags.Experimental)
        {
            var exp = flags.HasFlag(SpectrumExportFlags.Experimental);
            var deriv = flags.HasFlag(SpectrumExportFlags.Derivative);
            var integral = flags.HasFlag(SpectrumExportFlags.Integral);
            var csv = spc.Title;
            var xTitle = $"{spc.Quantity()}/{spc.Unit()}{separator}";
            csv += $"\n{xTitle}{spc.YQuantity()}{separator}" +
                   $"{(deriv ? $"{xTitle}Derivative" + separator : "")}" +
                   $"{(integral ? $"{xTitle}Integral" + separator : "")}\n";
            for (var i = 0; i < spc.XYData.Count; i++)
            {
                csv += $"{DataString(spc.XYData[i].X, separator, exp)}" +
                       $"{DataString(spc.XYData[i].Y, separator, exp)}" +
                       $"{DataString(spc.Derivative[i].X, separator, deriv)}" +
                       $"{DataString(spc.Derivative[i].Y, separator, deriv)}" +
                       $"{DataString(spc.Integral[i].X, separator, integral)}" +
                       $"{DataString(spc.Integral[i].Y, separator, integral)}\n";
            }
            return csv;
        }

        private static string DataString(double data, char separator, bool use = true) =>
            $"{(use ? data.ToInvariantString() + separator : "")}";

        public override void Export(IExportable exportable, Stream stream)
        {
            base.Export(exportable, stream);
            using var sw = new StreamWriter(stream);
            sw.Write(ExportToString(Spectrum, Separator, Flags));
        }
    }
}
