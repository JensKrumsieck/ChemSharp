using System;

namespace ChemSharp.Files
{
    public class Int32BinaryFile : BinaryFile
    {
        public int[] Int32Data
        {
            get;
        }
        public Int32BinaryFile (string path) : base(path)
        {
            Int32Data = new int[Data.Length / 4];
            Buffer.BlockCopy(Data, 0, Int32Data, 0, Data.Length);
        }
    }
}
