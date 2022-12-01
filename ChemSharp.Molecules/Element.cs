using System.Text.Json.Serialization;

namespace ChemSharp.Molecules;

/// <summary>
///     represents a chemical element
///     data taken from https://github.com/JensKrumsieck/periodic-table api
/// </summary>
public class Element
{
	public static readonly Dictionary<string, int> DesiredSaturation = new();

	/// <summary>
	///     Dummy Element
	/// </summary>
	private static readonly Element Dummy = new() {Symbol = "DA", Name = "Dummy Atom"};

	private static readonly string[] Metalloids = {"B", "Si", "Ge", "As", "Sb", "Bi", "Se", "Te", "Po"};
	private static readonly string[] NonMetals = {"H", "C", "N", "O", "P", "S", "Se"};

	[JsonIgnore] private string? _color;

	static Element()
	{
		var transitionGroups = new List<int>
		{
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12
		};
		foreach (var element in ElementDataProvider.ElementData)
		{
			if (element.Group == 0) continue;

			var saturation = transitionGroups.Contains(element.Group) ? 0 :
				element.Group <= 3 ? element.Group :
				element.Group <= 14 ? 4 : 8 - (element.Group - 10);
			DesiredSaturation.Add(element.Symbol, saturation);
		}
	}

	/// <summary>
	///     Constructor for Json Serialization
	/// </summary>
	[JsonConstructor]
#pragma warning disable CS8618
	public Element() { }
#pragma warning restore CS8618

	/// <summary>
	///     Create Element by symbol
	/// </summary>
	/// <param name="symbol"></param>
	public Element(string symbol)
	{
		if (symbol == "D") symbol = "H"; //filter deuterium
		var shadow = ElementDataProvider.ElementData.FirstOrDefault(s => s.Symbol == symbol) ?? Dummy;

		Name = shadow.Name;
		Symbol = shadow.Symbol;
		Appearance = shadow.Appearance;
		AtomicWeight = shadow.AtomicWeight;
		AtomicNumber = shadow.AtomicNumber;
		Group = shadow.Group;
		Period = shadow.Period;
		Block = shadow.Block;
		Category = shadow.Category;
		ElectronConfiguration = shadow.ElectronConfiguration;
		Electronegativity = shadow.Electronegativity;
		CovalentRadius = shadow.CovalentRadius;
		AtomicRadius = shadow.AtomicRadius;
		VdWRadius = shadow.VdWRadius;
		CAS = shadow.CAS;
	}

	/// <summary>
	///     Returns Element Color
	/// </summary>
	[JsonIgnore]
	public string Color => _color ??= ElementDataProvider.ColorData[Symbol];

	[JsonIgnore] public bool IsMetal => !IsMetalloid && !IsNonMetal;
	[JsonIgnore] public int Charge { get; set; }

	[JsonIgnore] public int Electrons => AtomicNumber - Charge;

	[JsonIgnore] public bool IsMetalloid => Metalloids.Contains(Symbol);

	[JsonIgnore] public bool IsNonMetal => NonMetals.Contains(Symbol) || Group is 18 or 17;
	public string Name { get; set; }
	public string Symbol { get; set; }
	public string Appearance { get; set; }
	public double AtomicWeight { get; set; }
	public int AtomicNumber { get; set; }
	public int Group { get; set; }
	public int Period { get; set; }
	public string Block { get; set; }
	public string Category { get; set; }
	public string ElectronConfiguration { get; set; }
	public double? Electronegativity { get; set; }
	public int CovalentRadius { get; set; }
	public int AtomicRadius { get; set; }
	public int? VdWRadius { get; set; }
	public string CAS { get; set; }
}
