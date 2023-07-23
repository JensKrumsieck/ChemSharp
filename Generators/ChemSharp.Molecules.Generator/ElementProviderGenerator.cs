using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ChemSharp.Molecules.Generator;

[Generator]
public class ElementProviderGenerator : ISourceGenerator
{
	private const string Indent = "    ";

	/// <summary>
	///     Where the colorData is located
	/// </summary>
	private const string ColorSource = "ChemSharp.Molecules.Generator.Resources.colorData.txt";

	/// <summary>
	///     Where the element data is located
	/// </summary>
	private const string ApiSource = "ChemSharp.Molecules.Generator.Resources.elements.json";

	public void Initialize(GeneratorInitializationContext context)
	{
		_elementData = EnsureApiData();
		_colorData = EnsureColorData();
	}
	public void Execute(GeneratorExecutionContext context)
	{
		var sb = new StringBuilder();
		sb.AppendLine("using System.Collections.Generic;");
		sb.Append(@$"namespace ChemSharp.Molecules{{
	public static class ElementDataProvider{{
		public static Dictionary<string, string> ColorData => _colorData;
		public static Element[] ElementData => _elementData;
		private static Element[] _elementData = new Element[]{{
");
		foreach (var jsonElement in _elementData)
		{
			sb.AppendLine(Space(3) + "new Element(){");
			sb.AppendLine(Space(4) + $"Name = \"{jsonElement.TryGetString("Name")}\",");
			sb.AppendLine(Space(4) + $"Symbol = \"{jsonElement.TryGetString("Symbol")}\",");
			sb.AppendLine(Space(4) + $"Appearance = \"{jsonElement.TryGetString("Appearance")}\",");
			sb.AppendLine(Space(4) + $"AtomicWeight = {jsonElement.TryGetDouble("AtomicWeight")},");
			sb.AppendLine(Space(4) + $"AtomicNumber = {jsonElement.TryGetInt("AtomicNumber")},");
			sb.AppendLine(Space(4) + $"Group = {jsonElement.TryGetInt("Group")},");
			sb.AppendLine(Space(4) + $"Period = {jsonElement.TryGetInt("Period")},");
			sb.AppendLine(Space(4) + $"Block = \"{jsonElement.TryGetString("Block")}\",");
			sb.AppendLine(Space(4) + $"Category = \"{jsonElement.TryGetString("Category")}\",");
			sb.AppendLine(
				Space(4) + $"ElectronConfiguration = \"{jsonElement.TryGetString("ElectronConfiguration")}\",");
			sb.AppendLine(Space(4) + $"Electronegativity = {jsonElement.TryGetDouble("Electronegativity")},");
			sb.AppendLine(Space(4) + $"CovalentRadius = {jsonElement.TryGetInt("CovalentRadius")},");
			sb.AppendLine(Space(4) + $"AtomicRadius = {jsonElement.TryGetInt("AtomicRadius")},");
			sb.AppendLine(Space(4) + $"VdWRadius = {jsonElement.TryGetInt("VdWRadius")},");
			sb.AppendLine(Space(4) + $"CAS = \"{jsonElement.TryGetString("CAS")}\",");
			sb.AppendLine(Space(3) + "},");
		}

		sb.Append(@$"
		}};

		private static Dictionary<string, string> _colorData = new Dictionary<string,string>(){{
");
		foreach (var kvp in _colorData) sb.AppendLine(Space(3) + $"{{\"{kvp.Key}\",\"{kvp.Value}\"}},");
		sb.Append(@$"
		}};
	}}
}}");
		context.AddSource($"ElementDataProvider_generated", SourceText.From(sb.ToString(), Encoding.UTF8));
	}

	private static JsonElement[] EnsureApiData()
	{
		var raw = ResourceUtil.ReadResourceString(ApiSource);
		return JsonSerializer.Deserialize<JsonElement[]>(raw)!;
	}
	/// <summary>
	///     Loads Color Data from ColorSource
	/// </summary>
	/// <returns></returns>
	private static ReadOnlyDictionary<string, string> EnsureColorData()
	{
		var data = ResourceUtil.ReadResourceString(ColorSource);
		var dic = data.Split(new[] {"\n", "\r\n", "\r"}, StringSplitOptions.RemoveEmptyEntries)
			.Select(line => line.Split(','))
			.ToDictionary(columns => columns[0], columns => columns[1]);
		return new ReadOnlyDictionary<string, string>(dic);
	}

	private static string Space(int spacers)
	{
		var res = new StringBuilder();
		for (var i = 0; i < spacers; i++)
			res.Append(Indent);
		return res.ToString();
	}

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
	private JsonElement[] _elementData;
	private ReadOnlyDictionary<string, string> _colorData;
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
}
