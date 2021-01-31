using ChemSharp.Molecules;
using ChemSharp.Rendering.Primitives;

namespace ChemSharp.Rendering
{
    public class SvgAtom
    {
        protected Atom Atom;

        public ISvgItem Tag => new SvgText
        {
            Fill = Atom.Color,
            ActualFontSize = 14,
            FontFamily = "Arial",
            HorizontalAlign = "middle",
            VerticalAlign = "central",
            X = Atom.Location.X * Constants.AngstromToPixels,
            Y = Atom.Location.Y * Constants.AngstromToPixels,
            Text = Atom.Symbol
        };

        public SvgAtom(Atom atom)
        { Atom = atom; }
    }
}


