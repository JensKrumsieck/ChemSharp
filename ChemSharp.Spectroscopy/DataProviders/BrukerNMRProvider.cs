using System.Numerics;
using ChemSharp.DataProviders;
using ChemSharp.Extensions;
using ChemSharp.Files;
using ChemSharp.Mathematics;

namespace ChemSharp.Spectroscopy.DataProviders;

public class BrukerNMRProvider : AbstractXYDataProvider, IParameterProvider
{
	/// <summary>
	///     You can force the library to process the fid file instead of 1r/1i if they are present
	/// </summary>
	private readonly bool _forceFID;

	static BrukerNMRProvider()
	{
		if (!FileHandler.RecipeDictionary.ContainsKey("acqus"))
			FileHandler.RecipeDictionary.Add("acqus", s => new ParameterFile(s, '='));

		if (!FileHandler.RecipeDictionary.ContainsKey("procs"))
			FileHandler.RecipeDictionary.Add("procs", s => new ParameterFile(s, '='));

		if (!FileHandler.RecipeDictionary.ContainsKey("fid"))
			FileHandler.RecipeDictionary.Add("fid", s => new PlainFile<int>(s));

		if (!FileHandler.RecipeDictionary.ContainsKey("1r"))
			FileHandler.RecipeDictionary.Add("1r", s => new PlainFile<int>(s));

		if (!FileHandler.RecipeDictionary.ContainsKey("1i"))
			FileHandler.RecipeDictionary.Add("1i", s => new PlainFile<int>(s));
	}

	/// <summary>
	///     ctor
	/// </summary>
	/// <param name="path"></param>
	/// <param name="forceFid">Use FID instead of processed files</param>
	public BrukerNMRProvider(string path, bool forceFid = false) : base(path)
	{
		_forceFID = forceFid;
		SelectStrategy(path);
	}

	/// <summary>
	///     default ctor
	/// </summary>
	/// <param name="path"></param>
	public BrukerNMRProvider(string path) : this(path, false) { }

	/// <inheritdoc />
	public IDictionary<string, string> Storage { get; set; }

	/// <inheritdoc />
	public string this[string index] => Storage[index];

	/// <summary>
	///     Selects which strategy to use for XYData Generation
	/// </summary>
	/// <param name="path"></param>
	/// <exception cref="InvalidDataException"></exception>
	private void SelectStrategy(string path)
	{
		var builder = PathBuilder(path);
		Storage = ((ParameterFile)FileHandler.Handle(builder["acqus"])).Storage;

		var sw = this["##$SW_h"].ToDouble();
		var cnt = this["##$TD"].ToInt() / 2;
		var freq = this["##$SFO1"].ToDouble() * 1e6;

		//array needs to be power of 2
		var fftSize = MathUtil.PowerOf2(cnt) ? cnt : MathUtil.NextPowerOf2(cnt);
		var freqData = CollectionsUtil.LinearRange(-sw / 2, sw / 2, cnt).ToArray();

		//get ppm scale
		var ppmData = fftSize == cnt
			? freqData.Select(s => s / freq * 1e6).ToArray()
			: CollectionsUtil.LinearRange(-sw / 2, sw / 2, fftSize).Select(s => s / freq * 1e6f).ToArray();

		if (File.Exists(builder["procs"]))
		{
			Storage.Add("##$OFFSET", ((ParameterFile)FileHandler.Handle(builder["procs"])).Storage["##$OFFSET"]);
			var max = ppmData.Max();
			ppmData = ppmData.Select(s => s + this["##$OFFSET"].ToDouble() - max).ToArray();
		}

		//use processed data, 1r and 1i contain processed nmr data where 1r contains the real and 1i the imaginary part.
		if (File.Exists(builder["1i"]) && File.Exists(builder["1r"]) && !_forceFID)
			XYData = DataPoint.FromDoubles(ppmData, HandleProcessed(builder).Reverse().ToArray()).ToArray();
		else
			XYData = DataPoint.FromDoubles(ppmData, HandleFid(builder["fid"]).ToArray()).ToArray();
	}

	/// <summary>
	///     builds path to open files
	/// </summary>
	/// <param name="filename"></param>
	/// <returns></returns>
	private static Dictionary<string, string> PathBuilder(string filename)
	{
		var path = System.IO.Path.GetDirectoryName(filename);
		if (path != null && path.Contains("pdata"))
			path = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @"..\..\"));

		var dic = new Dictionary<string, string>
		{
			{"fid", path + "\\fid"},
			{"acqus", path + "\\acqus"},
			{"1r", path + "\\pdata\\1\\1r"},
			{"1i", path + "\\pdata\\1\\1r"},
			{"procs", path + "\\pdata\\1\\procs"}
		};
		return dic;
	}

	/// <summary>
	///     Handles FID
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	private static IEnumerable<double> HandleFid(string path)
	{
		var file = (PlainFile<int>)FileHandler.Handle(path);
		var cmplx = file.Content.ToComplexes().ToArray();
		var fourier = cmplx.Radix2FFT().ToInts().FftShift().ToDoubles();
		return fourier;
	}

	/// <summary>
	///     Handles 1r/1i
	/// </summary>
	/// <param name="builder"></param>
	/// <returns></returns>
	private static IEnumerable<double> HandleProcessed(Dictionary<string, string> builder)
	{
		var oneR = (PlainFile<int>)FileHandler.Handle(builder["1r"]);
		var oneI = (PlainFile<int>)FileHandler.Handle(builder["1i"]);
		for (var i = 0; i < oneR.Content.Length; i++)
			yield return new Complex(oneR.Content[i], oneI.Content[i]).Magnitude;
	}
}
