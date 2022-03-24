using System;
using System.Collections.Generic;
using ChemSharp.DataProviders;
using ChemSharp.Files;
using ChemSharp.Spectroscopy.DataProviders;

namespace ChemSharp.Spectroscopy;

public static class SpectrumFactory
{
	/// <summary>
	///     Contains a file extension - type relationship
	/// </summary>
	public static readonly Dictionary<string, Type> DataProviderDictionary = new();

	/// <summary>
	///     Import supported types for auto creation
	/// </summary>
	static SpectrumFactory()
	{
		DataProviderDictionary.Add("par", typeof(BrukerEPRProvider));
		DataProviderDictionary.Add("spc", typeof(BrukerEPRProvider));
		DataProviderDictionary.Add("dsw", typeof(VarianUVVisProvider));
		DataProviderDictionary.Add("fid", typeof(BrukerNMRProvider));
		DataProviderDictionary.Add("1r", typeof(BrukerNMRProvider));
		DataProviderDictionary.Add("1i", typeof(BrukerNMRProvider));
		DataProviderDictionary.Add("acqus", typeof(BrukerNMRProvider));
		DataProviderDictionary.Add("procs", typeof(BrukerNMRProvider));
	}

	/// <summary>
	///     Creates a Spectrum object with auto type detection
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static Spectrum Create(string path) => new((IXYDataProvider)CreateProvider(path));

	/// <summary>
	///     Handles IDataProvider creation
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static AbstractDataProvider CreateProvider(string path) =>
		FileHandler.CreateProvider<AbstractXYDataProvider>(path, DataProviderDictionary);
}
