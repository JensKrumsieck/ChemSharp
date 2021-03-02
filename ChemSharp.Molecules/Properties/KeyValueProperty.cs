﻿using System;

namespace ChemSharp.Molecules.Properties
{
    public class KeyValueProperty : IComparable<KeyValueProperty>
    {
        public virtual string Key { get; set; }
        public virtual float Value { get; set; }
        public virtual string Unit { get; set; }

        public override string ToString() => $"{Key}: {Value:N3} {Unit}";

        public int CompareTo(KeyValueProperty other)
        {
            if (ReferenceEquals(this, other)) return 0;
            return other is null ? 1 : Value.CompareTo(other.Value);
        }
    }
}