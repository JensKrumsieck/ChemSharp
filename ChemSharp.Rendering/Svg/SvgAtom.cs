using ChemSharp.Molecules;
using ChemSharp.Rendering.Primitives.Svg;
using System;

namespace ChemSharp.Rendering.Svg
{
    [Obsolete("Package will be removed soon")]
    public class SvgAtom
    {
        public readonly Atom Atom;

        public bool HasImplicitHydrogen;

        public ISvgItem Tag => new SvgText
        {
            Fill = Atom.Color,
            ActualFontSize = 14,
            FontFamily = "Arial",
            HorizontalAlign = "middle",
            VerticalAlign = "central",
            X = Atom.Location.X * Constants.AngstromToPixels,
            Y = Atom.Location.Y * Constants.AngstromToPixels,
            Text = Atom.Symbol + (HasImplicitHydrogen ? "H" : "")
        };

        public SvgAtom(Atom atom) { Atom = atom; }
    }
}


