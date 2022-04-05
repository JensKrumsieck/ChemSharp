using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using ChemSharp.Extensions;

namespace ChemSharp.Molecules.DataProviders;

public static class ElementDataProvider
{
	/// <summary>
	///     Where the colorData is located
	/// </summary>
	private const string ColorSource = "ChemSharp.Resources.colorData.txt";

	/// <summary>
	///     Where the element data is located
	/// </summary>
	public const string ApiSource = "ChemSharp.Resources.elements.json";

	private static ReadOnlyDictionary<string, string>? _colorData;
	private static Element[]? _elementData;

	static ElementDataProvider()
	{
		EnsureColorData();
		EnsureApiData();
	}

	/// <summary>
	///     contains color data
	/// </summary>
	public static ReadOnlyDictionary<string, string> ColorData
	{
		get
		{
			if (_colorData == null) EnsureColorData();
			return _colorData!;
		}
		private set => _colorData = value;
	}

	/// <summary>
	///     contains elemental data
	/// </summary>
	public static Element[] ElementData
	{
		get
		{
			if (_elementData == null) EnsureApiData();
			return _elementData!;
		}
		private set => _elementData = value;
	}

	/// <summary>
	///     Loads Color Data from ColorSource
	/// </summary>
	/// <returns></returns>
	public static void EnsureColorData()
	{
		var data = ResourceUtil.ReadResourceString(ColorSource);
		var dic = data.DefaultSplit()
		              .Select(line => line.Split(','))
		              .ToDictionary(columns => columns[0], columns => columns[1]);
		ColorData = new ReadOnlyDictionary<string, string>(dic);
	}

	/// <summary>
	///     Loads Element Data from ApiSource
	/// </summary>
	/// <returns></returns>
	public static void EnsureApiData()
	{
		//Read Data from https://github.com/JensKrumsieck/periodic-table
		//fetched from http://en.wikipedia.org
		var raw = ResourceUtil.ReadResourceString(ApiSource);
		var data = JsonSerializer.Deserialize<Element[]>(raw)!;
		ElementData = data;
	}
}
