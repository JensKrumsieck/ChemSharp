using ChemSharp.Molecules;
#if NET5_0_OR_GREATER || NETSTANDARD2_1
using System;
#endif
using System.Collections.Generic;
using System.Numerics;
using ChemSharp.Rendering.Primitives.Svg;

#if NETSTANDARD2_0
using ChemSharp.Mathematics;
using System;
#endif

namespace ChemSharp.Rendering.Svg
{
    [Obsolete("Package will be removed soon")]
    public class SvgBond
    {
        public readonly Bond Bond;

        public SvgBond(Bond bond) { Bond = bond; }

        public ISvgItem Tag => CreateTag();

        private ISvgItem CreateTag()
        {
            var atom1 = Bond.Atom1.Location * Constants.AngstromToPixels;
            var atom2 = Bond.Atom2.Location * Constants.AngstromToPixels;
            var start = new Vector2(atom1.X, atom1.Y);
            var end = new Vector2(atom2.X, atom2.Y);

            var realStart = start;
            var realEnd = end;

            if (Bond.Atom1.Symbol != "C")
                realStart = CalculateSpacing(start, end, 9);
            if (Bond.Atom2.Symbol != "C")
                realEnd = CalculateSpacing(end, start, 9);

            var path = new SvgPath { Stroke = "#000000", StrokeThickness = 2 };
            path.PathPartList.AddRange(AddSingleBondParts(realStart, realEnd));
            if (Bond.Order == 2 || Bond.Order == 3) path.PathPartList.AddRange(AddMultipleBondParts(realStart, realEnd));
            if (Bond.Order == 3) path.PathPartList.AddRange(AddMultipleBondParts(realStart, realEnd, 1));
            path.PathPartList.Add(SvgPathPart.ClosePart);
            return path;
        }



        private static IEnumerable<SvgPathPart> AddSingleBondParts(Vector2 start, Vector2 end)
        {
            yield return new SvgPathPart
            {
                Parameters = { start.X, start.Y },
                SvgPartType = SvgPartType.MoveTo
            };
            yield return new SvgPathPart
            {
                Parameters = { end.X, end.Y },
                SvgPartType = SvgPartType.LineTo
            };
        }

        private static IEnumerable<SvgPathPart> AddMultipleBondParts(Vector2 start, Vector2 end, int i = -1)
        {
            var m = (start.Y - end.Y) / (start.X - end.X);
            var mp = -1 / m;
            var dx = MathF.Cos(MathF.Atan(mp));
            var dy = MathF.Sin(MathF.Atan(mp));
            var spacer = i * 4f;

            var ds = new Vector2(start.X + spacer * dx, start.Y + spacer * dy);
            var de = new Vector2(end.X + spacer * dx, end.Y + spacer * dy);

            ds = CalculateSpacing(ds, de, 2);
            de = CalculateSpacing(de, ds, 2);

            yield return new SvgPathPart
            {
                Parameters = { ds.X, ds.Y },
                SvgPartType = SvgPartType.MoveTo
            };
            yield return new SvgPathPart
            {
                Parameters = { de.X, de.Y },
                SvgPartType = SvgPartType.LineTo
            };
        }

        private static Vector2 CalculateSpacing(Vector2 start, Vector2 end, float spacing)
        {
            var vec = end - start;
            var nVec = Vector2.Normalize(vec);
            var newVec = nVec * spacing;
            return start + newVec;
        }
    }
}
