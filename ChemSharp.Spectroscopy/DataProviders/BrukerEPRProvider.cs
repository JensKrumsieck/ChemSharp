using ChemSharp.DataProviders;
using ChemSharp.Extensions;
using ChemSharp.Files;

namespace ChemSharp.Spectroscopy.DataProviders;

public class BrukerEPRProvider : AbstractXYDataProvider, IParameterProvider
{
	/// <summary>
	///     import recipes
	/// </summary>
	static BrukerEPRProvider()
	{
		if (!FileHandler.RecipeDictionary.ContainsKey("par"))
			FileHandler.RecipeDictionary.Add("par", s => new ParameterFile(s, ' '));

		if (!FileHandler.RecipeDictionary.ContainsKey("spc"))
			FileHandler.RecipeDictionary.Add("spc", s => new PlainFile<float>(s));
	}

	/// <summary>
	///     ctor
	/// </summary>
	/// <param name="path"></param>
	public BrukerEPRProvider(string path) : base(path)
	{
		var filePath = System.IO.Path.GetDirectoryName(path) + "/" + System.IO.Path.GetFileNameWithoutExtension(path);
		XYData = HandleData(filePath).ToArray();
	}

	/// <summary>
	///     <inheritdoc />
	/// </summary>
	public IDictionary<string, string> Storage { get; set; }

	/// <summary>
	///     <inheritdoc />
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public string this[string index] => Storage[index];

	/// <summary>
	///     calculates x axis
	/// </summary>
	/// <param name="path"></param>
	private IEnumerable<DataPoint> HandleData(string path)
	{
		var parFile = (ParameterFile)FileHandler.Handle(path + ".par");
		var spcFile = (PlainFile<float>)FileHandler.Handle(path + ".spc");

		//copy storage
		Storage = parFile.Storage;

		var res = this["RES"].ToInt();
		var hcf = this["HCF"].ToDouble();
		var hsw = this["HSW"].ToDouble();

		var min = hcf - hsw / 2;
		var d = hsw / (res - 1);

		for (var i = 0; i < res; i++) yield return new DataPoint(min + d * i, Convert.ToDouble(spcFile.Content[i]));
	}
}
