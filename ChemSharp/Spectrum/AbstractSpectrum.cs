using System.Collections.Generic;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    public class AbstractSpectrum : ISpectrum
    {
        public Vector2[] Data { get; internal set; }

        /// <summary>
        /// associated files
        /// </summary>
        public IList<string> Files { get; set; } = new List<string>();
    }
}
