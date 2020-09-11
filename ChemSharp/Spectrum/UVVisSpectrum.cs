using System.Collections.Generic;
using System.Linq;
using ChemSharp.Math.Unit;

namespace ChemSharp.Spectrum
{
    public class UVVisSpectrum : AbstractSpectrum
    {
        public override void OnInit()
        {
            base.OnInit();
            SecondaryXAxis = EnergyAxis.ToArray();
        }

        public IEnumerable<float> EnergyAxis
        {
            get
            {
                var converter = new EnergyUnitConverter("nm", "cm^-1");
                for (var i = 0; i < Data.Length; i++) yield return (float)converter.Convert(Data[i].X);
            }
        }
    }
}
