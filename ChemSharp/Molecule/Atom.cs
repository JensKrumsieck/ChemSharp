using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ChemSharp.Molecule
{
    /// <summary>
    /// Represents an Atom : Element in the 3D Space
    /// </summary>
    public class Atom : Element, IEquatable<Atom>
    {
        /// <summary>
        /// See BondTo Method
        /// </summary>
        private const float Delta = 5f;

        /// <summary>
        /// Location in 3D Space
        /// </summary>
        public Vector3 Location { get; set; }

        /// <inheritdoc cref="Element"/>
        public Atom(string symbol) : base(symbol)
        {
        }

        /// <summary>
        /// Computes Distance To Other Atom
        /// Wrapper fro Vector3.Distance
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public float DistanceTo(Atom test) => Vector3.Distance(Location, test.Location);

        /// <summary>
        /// Tests if Atom is Bond to another based on distance!
        /// allow uncertainity of 5 pm overall
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        // ReSharper disable possibleInvalidOperationException
        public bool BondTo(Atom test) => DistanceTo(test) < ((float)CovalentRadius + (float)test.CovalentRadius + Delta) / 100f;

        private string _title;
        /// <summary>
        /// Gets or Sets the Atom title
        /// </summary>
        public string Title
        {
            get => !string.IsNullOrEmpty(_title) ? _title : Symbol;
            set => _title = value;
        }


        public override string ToString() => Title;

        /// <summary>
        /// Gets HashCode, defined by title and location
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() => HashCode.Combine(_title, Location);

        public bool Equals(Atom other) =>
            !(other is null) &&
            (ReferenceEquals(this, other)
             || _title == other._title && Location.Equals(other.Location));

        public override bool Equals(object obj) =>
            !(obj is null) &&
            (ReferenceEquals(this, obj)
             || obj.GetType() == this.GetType() && Equals((Atom)obj));
    }
}
