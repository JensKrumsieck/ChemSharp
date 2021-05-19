using ChemSharp.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Rendering.Primitives.SVG
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
        public List<double> Parameters = new();

        public override string ToString() => $"{(char)SvgPartType} {(Parameters != null ? string.Join(",", Parameters.Select(s => s.ToInvariantString())) : "")}";

        public static SvgPathPart ClosePart => new() { SvgPartType = SvgPartType.ClosePath };
    }

}
