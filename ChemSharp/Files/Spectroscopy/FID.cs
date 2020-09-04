using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChemSharp.Files.Spectroscopy
{
    public class FID : BinaryFile
    {
        public Complex[] YData { get; set; }
        public FID(string path) : base(path)
        {
            var data = new int[Data.Length / 4];
            Buffer.BlockCopy(Data, 0, data, 0, data.Length);
            //odd is real, even is imag
            YData = ReadData(data);
        }

        private static Complex[] ReadData(IReadOnlyList<int> data)
        {
            var complex = new Complex[data.Count/2];
            for (var i = 0; i < data.Count/2; i++) 
                complex[i] = new Complex(data[2 * i], data[2 * i + 1]);

            return complex;
        }
    }
}
