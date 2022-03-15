using System;
using System.Numerics;

namespace ChemSharp.Molecules;

/// <summary>
/// Atom represents an Element in 3d Space
/// </summary>
public class Atom : Element, IEquatable<Atom>
{
    /// <summary>
    /// Location in 3D Space
    /// </summary>
    public Vector3 Location { get; set; }

    /// <inheritdoc />
    public Atom(string symbol) : base(symbol) { }

    /// <summary>
    /// Computes Distance To Other Atom
    /// Wrapper from Vector3.Distance
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public float DistanceTo(Atom test) => Vector3.Distance(Location, test.Location);

    private string _title;
    /// <summary>
    /// Gets or Sets the Atom title
    /// </summary>
    public string Title
    {
        get => !string.IsNullOrEmpty(_title) ? _title : Symbol;
        set => _title = value;
    }

    public override string ToString() => $"{Title}: {Location}";

    /// <summary>
    /// Gets HashCode, defined by title and location
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (_title, Location).GetHashCode();

    public bool Equals(Atom other) =>
        other is not null &&
        (ReferenceEquals(this, other)
         || _title == other._title && Location.Equals(other.Location));

    public override bool Equals(object obj) =>
        obj is not null &&
        (ReferenceEquals(this, obj)
         || obj.GetType() == GetType() && Equals((Atom)obj));

    /// <summary>
    /// Allows to tag any string related information to atom
    /// </summary>
    public string Tag;
}
