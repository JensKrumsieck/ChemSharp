using ChemSharp.DataProviders;

namespace ChemSharp.Spectroscopy.DataProviders;

/// <summary>
///     Uses MultiCSVProvider to directly create XYDataProvider from first item
/// </summary>
public class GenericCSVProvider : IXYDataProvider, IParameterProvider
{
	/// <summary>
	///     Common CSV Translations
	/// </summary>
	private static readonly Dictionary<string, string> Translations = new() {{"Wavelength", "λ"}, {"Abs", "rel. Abs."}};

	/// <summary>
	///     <inheritdoc cref="MultiCSVProvider" />
	/// </summary>
	/// <param name="path"></param>
	/// <param name="delimiter"></param>
	/// <param name="headerPos"></param>
	/// <param name="dataOffset"></param>
	public GenericCSVProvider(string path, char delimiter = ',', int dataOffset = 0)
	{
		Path = path;
		var multiCSV = new MultiCSVProvider(path, delimiter);
		XYData = multiCSV.MultiXYData[dataOffset];
		var (xHeader, yHeader) = multiCSV.XYHeaders[dataOffset];

		var xHeaderArr = xHeader.Split(' ');
		xHeader = xHeaderArr[0];
		var unit = "";
		if (xHeaderArr.Length > 1) unit = xHeaderArr[1];

		Storage.Add("Unit", unit);

		//use translations
		if (Translations.ContainsKey(yHeader)) yHeader = Translations[yHeader];

		if (Translations.ContainsKey(xHeader)) xHeader = Translations[xHeader];

		Storage.Add("XHeader", xHeader);
		Storage.Add("YHeader", yHeader);
	}

	/// <inheritdoc />
	public IDictionary<string, string> Storage { get; set; } = new Dictionary<string, string>();

	/// <inheritdoc />
	public string this[string index] => Storage[index];

	/// <inheritdoc />
	public DataPoint[] XYData { get; set; }

	/// <inheritdoc />
	public string Path { get; set; }
}
