using System;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// Bruker EMX EPR Spectrometer File Type
    /// </summary>
    public class SPC : BinaryFile, IYSpectrumFile
    {
        public float[] YData { get; set; }

        public SPC(string path) : base(path)
        {
            YData = new float[Data.Length / 4];
            Buffer.BlockCopy(Data, 0, YData, 0, YData.Length);
        }

    }
}
