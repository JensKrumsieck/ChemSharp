using ChemSharp.Extensions;
using ChemSharp.Spectroscopy.Extensions;

namespace ChemSharp.Spectroscopy.Formats;

public class BrukerEPRFormat : FileFormat
{
	private Dictionary<string, string> _storage;
	private DataPoint[] XYData;

	private BrukerEPRFormat()
	{
		ValidationMethod = FileExtensions.ValidateWithExtensions;
		NeededFiles = new Dictionary<string, Action<string>> {{".par", ReadPAR}, {".spc", ReadSPC}};
	}

	private void ReadPAR(string file) =>
		//files are usually short, can get away with reading all at once
		_storage = FileExtensions.ReadStorageFile(file, ' ');

	private void ReadSPC(string file)
	{
		var res = _storage["RES"].ToInt();
		var hcf = _storage["HCF"].ToDouble();
		var hsw = _storage["HSW"].ToDouble();
		var min = hcf - hsw / 2;
		var d = hsw / (res - 1);
		XYData = new DataPoint[res];
		var yData = new float[res];
		var rawBytes = File.ReadAllBytes(file);
		Buffer.BlockCopy(rawBytes, 0, yData, 0, rawBytes.Length);

		for (var i = 0; i < res; i++) XYData[i] = new DataPoint(min + d * i, yData[i]);
	}

	public static Spectrum Read(string filename)
	{
		var format = new BrukerEPRFormat();
		format.Load(filename);
		return new Spectrum(format.XYData) {Title = filename, optionalParameters = format._storage};
	}
}
