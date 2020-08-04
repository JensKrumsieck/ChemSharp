using System.Numerics;

namespace ChemSharp.Files.Spectroscopy
{
    public interface IXYSpectrumFile
    {
       Vector2[] XYData { get; set; }
    }
}
