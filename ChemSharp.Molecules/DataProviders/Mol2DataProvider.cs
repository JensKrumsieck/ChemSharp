using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using ChemSharp.Extensions;
using ChemSharp.Files;

namespace ChemSharp.Molecules.DataProviders;

[Obsolete("Use Format instead! Will be removed in 1.2.0")]
public class Mol2DataProvider : AbstractAtomDataProvider, IBondDataProvider
{
	/// <summary>
	///     import recipes
	/// </summary>
	static Mol2DataProvider()
	{
		if (!FileHandler.RecipeDictionary.ContainsKey("mol2"))
			FileHandler.RecipeDictionary.Add("mol2", s => new PlainFile<string>(s));
	}

	public Mol2DataProvider(string path) : base(path) => ReadData();

	public Mol2DataProvider(Stream stream) : base(stream) => ReadData();

	public IEnumerable<Bond> Bonds { get; set; }

	public sealed override void ReadData()
	{
		var blocks = TriposBlocks(Content).ToArray();
		var atomBlock = Array.Find(blocks, s => s.Contains("ATOM")).DefaultSplit();
		var bondBlock = Array.Find(blocks, s => s.Contains("BOND")).DefaultSplit();
		Atoms = Read<Atom>(atomBlock).ToList();
		Bonds = Read<Bond>(bondBlock).ToList();
	}

	/// <summary>
	///     Reads Data
	/// </summary>
	/// <typeparam name="T">Atom or Bond!</typeparam>
	/// <param name="block"></param>
	/// <returns></returns>
	private IEnumerable<T> Read<T>(IEnumerable<string> block) where T : class
	{
		if (typeof(T) != typeof(Atom) && typeof(T) != typeof(Bond)) yield break;

		foreach (var line in block)
		{
			if (string.IsNullOrEmpty(line) || line is "ATOM" or "BOND") continue;

			var cols = line.WhiteSpaceSplit();
			if (typeof(T) == typeof(Atom))
				yield return ReadAtom(cols) as T;
			else
				yield return ReadBond(cols) as T;
		}
	}

	/// <summary>
	///     Reads Atom
	/// </summary>
	/// <param name="cols"></param>
	/// <returns></returns>
	private static Atom ReadAtom(IReadOnlyList<string> cols)
	{
		var id = cols[1];
		var loc = new Vector3(
		                      cols[2].ToSingle(),
		                      cols[3].ToSingle(),
		                      cols[4].ToSingle()
		                     );
		var type = cols[5];
		//check if "correct" sybyl type is given, use id otherwise
		var sym = RegexUtil.PointMatch.IsMatch(type) || ElementDataProvider.ColorData.ContainsKey(type)
			? RegexUtil.AtomLabel.Match(type).Value
			: RegexUtil.AtomLabel.Match(id).Value;

		return new Atom(sym) {Title = id, Location = loc, Tag = type};
	}

	/// <summary>
	///     Reads Bond
	/// </summary>
	/// <param name="cols"></param>
	/// <returns></returns>
	private Bond ReadBond(IReadOnlyList<string> cols)
	{
		//subtract 1 as mol2 starts counting at 1
		var a1 = cols[1].ToInt() - 1;
		var a2 = cols[2].ToInt() - 1;
		var bond = new Bond(Atoms.ElementAt(a1), Atoms.ElementAt(a2));
		var type = cols[3];
		if (type == "ar") bond.IsAromatic = true;

		if (int.TryParse(type, out var order)) bond.Order = order;

		return bond;
	}


	/// <summary>
	///     reorders text into tripos blocks
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	private static IEnumerable<string> TriposBlocks(string[] data)
	{
		var text = string.Join("\n", data);
		return text.Split(new[] {"@<TRIPOS>"}, StringSplitOptions.None);
	}
}
