using System;
using System.Numerics;

namespace ChemSharp.Molecules;

/// <summary>
///     Atom represents an Element in 3d Space
/// </summary>
public class Atom : Element, IEquatable<Atom>
{
	private string _title = "";

	/// <summary>
	///     Allows to tag any string related information to atom
	/// </summary>
	public string? Tag;

	/// <inheritdoc />
	public Atom(string symbol) : base(symbol) { }

	public Atom(string symbol, float x, float y, float z) : this(symbol) => Location = new Vector3(x, y, z);

	/// <summary>
	///     Location in 3D Space
	/// </summary>
	public Vector3 Location { get; set; }

	/// <summary>
	///     Gets or Sets the Atom title
	/// </summary>
	public string Title
	{
		get => !string.IsNullOrEmpty(_title) ? _title : Symbol;
		set => _title = value;
	}

	public bool Equals(Atom? other) =>
		other is not null &&
		(ReferenceEquals(this, other)
		 || Symbol == other.Symbol && Location.Equals(other.Location));

	/// <summary>
	///     Computes Distance To Other Atom
	///     Wrapper from Vector3.Distance
	/// </summary>
	/// <param name="test"></param>
	/// <returns></returns>
	public float DistanceTo(Atom test) => Vector3.Distance(Location, test.Location);

	/// <summary>
	///     Returns squared Distance of two atoms
	/// </summary>
	/// <param name="test"></param>
	/// <returns></returns>
	public float DistanceToSquared(Atom test) => Vector3.DistanceSquared(Location, test.Location);

	public override string ToString() => $"{Title}: {Location}";

	/// <summary>
	///     Gets HashCode, defined by Symbol and location
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode() => (Title, Symbol, Location).GetHashCode();

	public override bool Equals(object? obj) =>
		obj is not null &&
		(ReferenceEquals(this, obj)
		 || obj.GetType() == GetType() && Equals((Atom)obj));

	public static bool operator ==(Atom? a, Atom? b) =>
		a is null && b is null
		|| a is not null && b is not null
		                 && (ReferenceEquals(a, b) || a.Equals(b));

	public static bool operator !=(Atom? a, Atom? b) =>
		a is null || b is null
		          || !ReferenceEquals(a, b)
		          && !a.Equals(b);
}
