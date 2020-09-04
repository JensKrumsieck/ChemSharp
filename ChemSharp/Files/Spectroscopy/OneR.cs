using System;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// 1R Files
    /// </summary>
    public class OneR : BinaryFile, IYSpectrumFile
    {
        public OneR(string path) : base(path)
        {
            var data = new int[Data.Length / 4];
            Buffer.BlockCopy(Data, 0, data, 0, data.Length);
            YData = data.Select(s => (float)s).ToArray();
        }

        public float[] YData { get; set; }
    }
}
