using ChemSharp.Molecules;
using ChemSharp.Rendering.Primitives;
using ChemSharp.Rendering.Primitives.SVG;

namespace ChemSharp.Rendering.Svg
{
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


