using System.Numerics;
using ChemSharp.Extensions;
using ChemSharp.Mathematics;
using ChemSharp.Spectroscopy.Extensions;

namespace ChemSharp.Spectroscopy.Formats;

public class BrukerNMRFormat : FileFormat
{
	private Dictionary<string, string> _storage;
	private int[]? oneI;
	private int[]? oneR;
	private double[] ppmData;

	private DataPoint[]? XYData;

	private BrukerNMRFormat()
	{
		ValidationMethod = FileExtensions.ValidateBrukerNMR;
		//this order ensures all data is already present
		NeededFiles = new Dictionary<string, Action<string>>
		{
			{"acqus", ReadAcqus},
			{"pdata/1/procs", ReadProcs},
			{"pdata/1/1r", Read1R},
			{"pdata/1/1i", Read1I},
			{"fid", ReadFid}
		};
	}

	private void ReadAcqus(string file) => _storage = FileExtensions.ReadStorageFile(file, '=');

	private void ReadProcs(string file)
	{
		if (!File.Exists(file)) return;
		var procsStorage = FileExtensions.ReadStorageFile(file, '=');
		//copy offset property from processed file
		_storage.Add("##$OFFSET", procsStorage["##$OFFSET"]);
	}

	private void ReadFid(string file)
	{
		//processed data already available return!
		if (XYData is not null) return;
		BuildPPMData();
		var rawBytes = File.ReadAllBytes(file);
		var fidData = new int[rawBytes.Length / sizeof(int)];
		Buffer.BlockCopy(rawBytes, 0, fidData, 0, rawBytes.Length);
		var cmplx = fidData.ToComplexes().ToArray();
		var fourier = cmplx.Radix2FFT().ToInts().FftShift().ToDoubles();
		XYData = new DataPoint[ppmData.Length];
		for (var i = 0; i < oneI.Length; i++)
			XYData[i] = new DataPoint(ppmData[i], fourier[i]);
	}

	private void Read1R(string file)
	{
		if (!File.Exists(file)) return;
		var rawBytes = File.ReadAllBytes(file);
		oneR = new int[rawBytes.Length / sizeof(int)];
		Buffer.BlockCopy(rawBytes, 0, oneR, 0, rawBytes.Length);
	}

	private void Read1I(string file)
	{
		//1r is read first
		if (!File.Exists(file) || oneR is null) return;
		var rawBytes = File.ReadAllBytes(file);
		oneI = new int[rawBytes.Length / sizeof(int)];
		Buffer.BlockCopy(rawBytes, 0, oneI, 0, rawBytes.Length);
		BuildPPMData();
		XYData = new DataPoint[ppmData.Length];
		var yData = new double[ppmData.Length];
		for (var i = 0; i < oneI.Length; i++)
			yData[i] = new Complex(oneR[i], oneI[i]).Magnitude;
		XYData = DataPoint.FromDoubles(ppmData, yData.Reverse().ToArray()).ToArray();
	}

	private void BuildPPMData()
	{
		var sw = _storage["##$SW_h"].ToDouble();
		var cnt = _storage["##$TD"].ToInt() / 2;
		var freq = _storage["##$SFO1"].ToDouble() * 1e6;
		var fftSize = MathUtil.PowerOf2(cnt) ? cnt : MathUtil.NextPowerOf2(cnt);
		var freqData = CollectionsUtil.LinearRange(-sw / 2, sw / 2, cnt);
		ppmData = (fftSize == cnt ? freqData : CollectionsUtil.LinearRange(-sw / 2, sw / 2, fftSize))
		          .Select(s => s / freq * 1e6).ToArray();
		//do correction if possible
		if (!_storage.ContainsKey("##$OFFSET")) return;
		var offset = _storage["##$OFFSET"].ToDouble();
		var max = ppmData.Max();
		ppmData = ppmData.Select(s => s + offset - max).ToArray();
	}

	public static Spectrum Read(string filename)
	{
		var format = new BrukerNMRFormat();
		format.Load(filename);
		return new Spectrum(format.XYData) {Title = filename, optionalParameters = format._storage};
	}
}
