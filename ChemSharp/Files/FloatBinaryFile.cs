using System;

namespace ChemSharp.Files
{
    public class FloatBinaryFile : BinaryFile
    {
        public float[] FloatData { get; }

        public FloatBinaryFile(string path) : base(path)
        {
            FloatData = new float[Data.Length / 4];
            Buffer.BlockCopy(Data, 0, FloatData, 0, FloatData.Length);
        }
    }
}
