using System.Collections.ObjectModel;

namespace ChemSharp.Spectroscopy
{
    public interface ISpectrum
    {
        /// <summary>
        /// The X and Y Axis data as collection of two doubles in the <see cref="DataPoint"/> struct
        /// </summary>
        public ObservableCollection<DataPoint> XYData { get; set; }

        /// <summary>
        /// Title of Spectrum
        /// </summary>
        public string Title { get; set; }
    }
}
