using System;
using System.Numerics;
using ChemSharp.Mathematics;
using ChemSharp.Molecules.Mathematics;

namespace ChemSharp.Molecules
{
    /// <summary>
    /// Atom represents an Element in 3d Space
    /// </summary>
    public class Atom : Element, IEquatable<Atom>
    {        
        /// <summary>
        /// See BondTo Method
        /// </summary>
        private const float Delta = 5f;

        /// <summary>
        /// Tests if Atom is Bond to another based on distance!
        /// allow uncertainty of 5 pm overall
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        internal bool InternalBondTo(Atom test)
        {
            if (CovalentRadius is null || test.CovalentRadius is null) return false;
            return DistanceTo(test) < (CovalentRadius + (float)test.CovalentRadius + Delta) / 100f;
        }

        private Vector3 _location;
        /// <summary>
        /// Location in 3D Space
        /// </summary>
        public Vector3 Location
        {
            get => Mapping(_location);
            set => _location = value;
        }

        /// <summary>
        /// Add a Mapping for Location to recalculate points
        /// into different coordinate systems
        /// </summary>
        public Func<Vector3, Vector3> Mapping { get; set; } = s => s;

        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="symbol"></param>
        public Atom(string symbol) : base(symbol) { }

        /// <summary>
        /// Computes Distance To Other Atom
        /// Wrapper from Vector3.Distance
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public float DistanceTo(Atom test) => MathV.Distance(Location, test.Location);

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
        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => HashCode.Combine(_title, _location);
        // ReSharper enable NonReadonlyMemberInGetHashCode

        public bool Equals(Atom other) =>
            !(other is null) &&
            (ReferenceEquals(this, other)
             || _title == other._title && Location.Equals(other._location));

        public override bool Equals(object obj) =>
            !(obj is null) &&
            (ReferenceEquals(this, obj)
             || obj.GetType() == this.GetType() && Equals((Atom)obj));
    }
}
