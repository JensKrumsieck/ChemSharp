﻿using System;
using System.Numerics;

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
        /// allow uncertainity of 5 pm overall
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        internal bool InternalBondTo(Atom test)
        {
            if (CovalentRadius is null || test.CovalentRadius is null) return false;
            return DistanceTo(test) < (CovalentRadius + (float)test.CovalentRadius + Delta) / 100f;
        }

        /// <summary>
        /// Location in 3D Space
        /// </summary>
        public Vector3 Location { get; set; }

        public Atom(string symbol) : base(symbol)
        { }

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
        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => HashCode.Combine(_title, Location);
        // ReSharper enable NonReadonlyMemberInGetHashCode

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
