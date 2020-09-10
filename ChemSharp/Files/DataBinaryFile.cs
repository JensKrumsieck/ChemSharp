using System;
using System.Runtime.InteropServices;

namespace ChemSharp.Files
{
    public class DataBinaryFile<T> : BinaryFile where T : struct
    {
        public T[] ConvertedData
        {
            get;
        }
        public DataBinaryFile(string path) : base(path)
        {
            ConvertedData = new T[Data.Length / Marshal.SizeOf<T>()];
            Buffer.BlockCopy(Data, 0, ConvertedData, 0, Data.Length);
        }
    }
}
