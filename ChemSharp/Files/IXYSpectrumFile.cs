using System.Numerics;

namespace ChemSharp.Files
{
    public interface IXYSpectrumFile
    {
       Vector2[] XYData { get; set; }
    }
}
