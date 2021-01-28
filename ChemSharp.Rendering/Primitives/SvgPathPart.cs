using System.Collections.Generic;

namespace ChemSharp.Rendering.Primitives
{
    public enum SvgPartType
    {
        MoveTo = 'M',
        LineTo = 'L',
        CubicBrezier = 'C',
        QuadraticBrezier = 'Q',
        EllipticalArc = 'A',
        ClosePath = 'Z'
    }

    public class SvgPathPart
    {
        public const string PartPattern = @"(M|L|C|A|Q|Z) ((\d+[.]?\d*)[,]?)*";

        public SvgPartType SvgPartType;
        public List<double> Parameters;

        public override string ToString() => $"{SvgPartType} {(Parameters != null ? string.Join(",", Parameters) : "")}";
    }
     
}
