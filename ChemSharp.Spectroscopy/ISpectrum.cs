using System.Collections.Generic;

namespace ChemSharp.Spectroscopy
{
    public interface ISpectrum
    {
        /// <summary>
        /// The X and Y Axis data as collection of two doubles in the <see cref="DataPoint"/> struct
        /// </summary>
        public List<DataPoint> XYData { get; set; }

        /// <summary>
        /// Title of Spectrum
        /// </summary>
        public string Title { get; set; }
    }
}
