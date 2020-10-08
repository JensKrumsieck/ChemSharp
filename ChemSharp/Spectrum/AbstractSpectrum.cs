using ChemSharp.Files;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    public class AbstractSpectrum : ISpectrum
    {

        public AbstractSpectrum()
        {
            Files.CollectionChanged += OnInit;
        }

        public ObservableCollection<Vector2> Data { get; set; } = new ObservableCollection<Vector2>();

        /// <summary>
        /// associated files
        /// </summary>
        public ObservableCollection<IFile> Files { get; set; } = new ObservableCollection<IFile>();

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
            for (var i = 0; i < Data.Count; i++)
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
            for (var i = 0; i < Data.Count; i++)
                yield return i == 0
                    ? Data[i]
                    : new Vector2(Data[i].X, Data[i].Y + Data[i - 1].Y);
        }

        /// <summary>
        /// will be called if files are added
        /// </summary>
        protected virtual void OnInit(object sender, NotifyCollectionChangedEventArgs args)
        {
            //does nothing for now
        }
    }
}