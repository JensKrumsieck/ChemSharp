using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    public class AbstractSpectrum : ISpectrum
    {
        public Vector2[] Data { get; internal set; }

        /// <summary>
        /// associated files
        /// </summary>
        public IList<string> Files { get; set; } = new List<string>();

        private Vector2[] _derivative;

        /// <summary>
        /// Gets the Derivative of Data
        /// </summary>
        public Vector2[] Derivative
        {
            get
            {
                if (_derivative.Equals(null)) _derivative = Derive().ToArray();
                return _derivative;
            }
        }

        private Vector2[] _integral;

        /// <summary>
        /// Gets the Integral of Data
        /// </summary>
        public Vector2[] Integral
        {
            get
            {
                if (_integral.Equals(null)) _integral = Integrate().ToArray();
                return _integral;
            }
        }

        /// <summary>
        /// Calculates the Derivative of Data
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector2> Derive()
        {
            for (var i = 0; i <= Data.Length; i++)
                yield return i == 0
                    ? Vector2.Zero
                    : new Vector2(Data[i].X, (Data[i].Y - Data[i - 1].Y) / (Data[i].X - Data[i - 1].X));
        }

        /// <summary>
        /// Calculates the Integral of Data
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector2> Integrate()
        {
            for (var i = 0; i <= Data.Length; i++)
                yield return i == 0 
                    ? Data[i] 
                    : new Vector2(Data[i].X, Data[i].Y + Data[i - 1].Y);
        }
    }
}
