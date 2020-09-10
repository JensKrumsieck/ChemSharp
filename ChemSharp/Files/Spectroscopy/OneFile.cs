using System.IO;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// Wrapper Class for 1r/1i files (processed Bruker NMR spectra)
    /// </summary>
    public class OneFile : Int32BinaryFile, IYSpectrumFile
    {
        /// <summary>
        /// The other part of 1r/1i files
        /// </summary>
        private Int32BinaryFile OtherPart { get; }

        public OneFile(string path) : base(path)
        {
            //check whether self is 1r or 1i
            var oldValue = Is1R ? "1r" : "1i";
            var newValue = Is1R ? "1i" : "1r";
            OtherPart = new Int32BinaryFile(Path.Replace(oldValue, newValue));
            if(OtherPart.Int32Data.Length != Int32Data.Length) 
                throw new InvalidDataException("The 1r/1i files do not match");
            YData = new float[Int32Data.Length];
            var real = Is1R ? Int32Data : OtherPart.Int32Data;
            var imag = Is1R ? OtherPart.Int32Data : Int32Data;
            for (var i = 0; i < YData.Length; i++)  YData[i] = (float) new Complex(real[i], imag[i]).Magnitude;
            YData = YData.Reverse().ToArray();
        }

        public float[] YData { get; set; }

        public bool Is1R => Path.Contains("1r");
    }
}
