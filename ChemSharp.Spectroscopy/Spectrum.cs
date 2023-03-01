using ChemSharp.Export;
using ChemSharp.Mathematics;

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
}
