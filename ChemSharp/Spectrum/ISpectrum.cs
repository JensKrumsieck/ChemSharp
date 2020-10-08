using System.Collections.ObjectModel;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    public interface ISpectrum
    {
        public ObservableCollection<Vector2> Data { get; set; }
    }
}
