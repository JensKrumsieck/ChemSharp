using ChemSharp.Math.Unit;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Spectrum
{
    public class UVVisSpectrum : AbstractSpectrum
    {
        public override void OnInit()
        {
            base.OnInit();
            SecondaryXAxis = EnergyAxis.ToArray();
        }

        /// <summary>
        /// Gets EnergyAxis;
        /// </summary>
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
