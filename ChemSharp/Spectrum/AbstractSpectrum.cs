using ChemSharp.Files;
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
        public IList<IFile> Files { get; set; } = new List<IFile>();

        private Vector2[] _derivative;

        /// <summary>
        /// Gets the Derivative of Data
        /// </summary>
        public Vector2[] Derivative
        {
            get { return _derivative ??= Derive().ToArray(); }
        }

        private Vector2[] _integral;

        /// <summary>
        /// Gets the Integral of Data
        /// </summary>
        public Vector2[] Integral
        {
            get { return _integral ??= Integrate().ToArray(); }
        }

        /// <summary>
        /// Calculates the Derivative of Data
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector2> Derive()
        {
            for (var i = 0; i < Data.Length; i++)
                yield return i == 0
                    ? new Vector2(Data[i].X, 0)
                    : new Vector2(Data[i].X, (Data[i].Y - Data[i - 1].Y) / (Data[i].X - Data[i - 1].X));
        }

        /// <summary>
        /// Calculates the Integral of Data
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector2> Integrate()
        {
            for (var i = 0; i < Data.Length; i++)
                yield return i == 0
                    ? Data[i]
                    : new Vector2(Data[i].X, Data[i].Y + Data[i - 1].Y);
        }

        /// <summary>
        /// Can be called after initialization through factory;
        /// </summary>
        public virtual void OnInit()
        {
            //does nothing
        }

        /// <summary>
        /// Dummy Method to link secondary x axis for plotting
        /// Dummy Method makes it exchangeable
        /// </summary>
        /// <returns></returns>
        public float[] SecondaryXAxis { get; set; }
    }
}