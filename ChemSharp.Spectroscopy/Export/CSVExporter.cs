namespace ChemSharp.Spectroscopy.Export;
/**
public class CSVExporter : AbstractSpectrumExporter
{
	/// <summary>
	///     CSV Separator
	/// </summary>
	public char Separator = ',';

	public static void Export(Spectrum spc, string filename, char separator,
	                          SpectrumExportFlags flags = SpectrumExportFlags.Experimental)
	{
		var exporter = new CSVExporter {Separator = separator, Flags = flags};
		using var stream = File.Create(filename);
		exporter.Export(spc, stream);
	}

	public static string ExportToString(Spectrum spc, char separator,
	                                    SpectrumExportFlags flags = SpectrumExportFlags.Experimental)
	{
		var exp = flags.HasFlag(SpectrumExportFlags.Experimental);
		var deriv = flags.HasFlag(SpectrumExportFlags.Derivative);
		var integral = flags.HasFlag(SpectrumExportFlags.Integral);

		var csv = new StringBuilder();
		csv.AppendLine(spc.Title);
		var xTitle = $"{spc.Quantity()}/{spc.Unit()}{separator}";
		csv.AppendLine($"{xTitle}{spc.YQuantity()}{separator}" +
		               $"{(deriv ? $"{xTitle}Derivative" + separator : "")}" +
		               $"{(integral ? $"{xTitle}Integral" + separator : "")}");

		for (var i = 0; i < spc.XYData.Count; i++)
		{
			var line = new StringBuilder();
			if (exp)
			{
				line.Append(spc.XYData[i].X.ToInvariantString() + separator);
				line.Append(spc.XYData[i].Y.ToInvariantString() + separator);
			}

			if (deriv)
			{
				line.Append(spc.Derivative[i].X.ToInvariantString() + separator);
				line.Append(spc.Derivative[i].Y.ToInvariantString() + separator);
			}

			if (integral)
			{
				line.Append(spc.Integral[i].X.ToInvariantString() + separator);
				line.Append(spc.Integral[i].Y.ToInvariantString() + separator);
			}

			csv.AppendLine(line.ToString());
		}

		return csv.ToString();
	}


	public override void Export(IExportable exportable, Stream stream)
	{
		base.Export(exportable, stream);
		using var sw = new StreamWriter(stream);
		sw.Write(ExportToString(Spectrum, Separator, Flags));
	}
}
**/
