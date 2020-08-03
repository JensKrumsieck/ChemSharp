using System.Numerics;
using ChemSharp.Files;

namespace ChemSharp.Spectrum
{
    public class AbstractSpectrum : ISpectrum
    {
        public Vector2[] Data { get; internal set; }
    }
}
