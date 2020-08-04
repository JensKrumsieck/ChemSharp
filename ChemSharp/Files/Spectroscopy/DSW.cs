using System;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// DSW Files from supported Devices:
    /// Varian Cary Scan
    /// </summary>
    public class DSW : BinaryFile, IXYSpectrumFile

    {
        private static int Entry = 0x459;
        private static int DataLength = 0x6d;

        public Vector2[] XYData { get; set; }

        public DSW(string path) : base(path)
        {
            var dataLength = BitConverter.ToInt32(Data, DataLength);
            //multiply length with 8, as each float32 contains 4 bytes and each data point is made of x and y data.
            var dataChunk = new ArraySegment<byte>(Data, Entry, dataLength * 8).ToArray();

            //read raw float data
            var result = new float[dataChunk.Length / 4];
            Buffer.BlockCopy(dataChunk, 0, result, 0, dataChunk.Length);

            XYData = new Vector2[result.Length / 2];
            for (int i = 0; i < result.Length; i += 2)
            {
                XYData[i / 2] = new Vector2(result[i], result[i + 1]);
            }
        }
    }
}
