using System.Collections.Generic;

namespace ChemSharp.Spectrum
{
    public class UVVisSpectrum : AbstractSpectrum
    {
        public IEnumerable<float> EnergyAxis
        {
            get
            {
                for (var i = 0; i < Data.Length; i++) yield return Data[i].X;
            }
        }
    }
}
