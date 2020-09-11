using System;
using System.Collections.Generic;
using System.Text;

namespace ChemSharp.Math.Unit
{
    public class MagneticUnitConverter : AbstractUnitConverter
    {
        public MagneticUnitConverter(string from, string to) : base(from, to)
        {
            AddConversion("T", 1);
            AddConversion("G", 1e-4);
            AddConversion("mT", 1e-3);
            AddConversion("Oe", 1e-4);
        }
    }
}
