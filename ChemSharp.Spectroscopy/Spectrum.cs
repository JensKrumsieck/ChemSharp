using ChemSharp.Export;
using ChemSharp.Extensions;
using ChemSharp.Mathematics;
using ChemSharp.Spectroscopy.Extensions;
using ChemSharp.Spectroscopy.Formats;

namespace ChemSharp.Spectroscopy;

public class Spectrum : ISpectrum, IExportable
{
	/// <summary>
	///     Backing field for <see cref="Spectrum.Derivative" />
	/// </summary>
	private List<DataPoint>? _derivative;

	/// <summary>
	///     Backing field for <see cref="Spectrum.Integral" />
	/// </summary>
	private List<DataPoint>? _integral;

	/// <summary>
	///     Quantity of X-Axis
	/// </summary>
	public string XQuantity = "X";

	/// <summary>
	///     Unit of X-Axis
	/// </summary>
	public string XUnit = "a.u.";

	public Spectrum(IEnumerable<DataPoint> dataPoints) => XYData = dataPoints.ToList();

	/// <summary>
	///     Derivative of XYData
	/// </summary>
	public List<DataPoint> Derivative => _derivative ??= XYData.Derive().ToList();

	/// <summary>
	///     Integral of XYData
	/// </summary>
	public List<DataPoint> Integral => _integral ??= XYData.Integrate().ToList();

	/// <summary>
	///     backing field for this Indexer
	/// </summary>
	internal Dictionary<string, string> optionalParameters { get; init; }

	/// <summary>
	///     Indexer for Properties
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public string this[string index] => optionalParameters[index];

	/// <summary>
	///     <inheritdoc cref="ISpectrum.XYData" />
	/// </summary>
	public List<DataPoint> XYData { get; set; } = new();

	/// <summary>
	///     <inheritdoc cref="ISpectrum.Title" />
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	///     Returns the Title
	/// </summary>
	/// <returns></returns>
	public override string ToString() => Title;

	public static Spectrum FromFile(string path)
	{
		var ext = FileUtil.GetExtension(path);
		if (FileExtensions.BrukerNMRFiles.Contains(ext)) return BrukerNMRFormat.Read(path);
		if (FileExtensions.BrukerEPRFiles.Contains(ext)) return BrukerEPRFormat.Read(path);
		if (FileExtensions.VarianUVVisFiles.Contains(ext)) return VarianUVVisFormat.Read(path);
		return FileExtensions.CSVFiles.Contains(ext) ? CSVFormat.Read(path, ',') : null;
	}
}
