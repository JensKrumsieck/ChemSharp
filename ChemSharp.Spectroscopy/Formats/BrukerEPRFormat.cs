﻿using ChemSharp.Extensions;
using ChemSharp.Spectroscopy.Extension;

namespace ChemSharp.Spectroscopy.Formats;

public class BrukerEPRFormat : SpectrumFormat
{
	private Dictionary<string, string> _storage;
	private DataPoint[] XYData;

	public BrukerEPRFormat()
	{
		ValidationMethod = FileExtensions.ValidateWithExtensions;
		NeededFiles = new Dictionary<string, Action<string>> {{".par", ReadPAR}, {".spc", ReadSPC}};
	}

	private void ReadPAR(string file)
	{
		//files are usually short, can get away with reading all at once
		var content = File.ReadAllLines(file);
		_storage = (from line in content
		            select line.Split(new[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries)
		            into raw
		            where raw.Length == 2
		            select new KeyValuePair<string, string>(raw[0].Trim(),
		                                                    raw[1].Trim()))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

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
		return new Spectrum(format.XYData) {Title = filename};
	}
}
