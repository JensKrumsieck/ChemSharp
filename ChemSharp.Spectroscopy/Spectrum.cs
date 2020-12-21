using ChemSharp.DataProviders;
using ChemSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChemSharp.Spectroscopy
{
    public class Spectrum : ISpectrum, IDataObject, INotifyPropertyChanged
    {
        public Spectrum()
        {
            XYData.CollectionChanged += OnXYChanged;
            PropertyChanged += OnPropertyChanged;
        }


        private IDataProvider _dataProvider;
        ///<summary>
        /// <inheritdoc />
        /// </summary>
        public IDataProvider DataProvider
        {
            get => _dataProvider;
            set
            {
                _dataProvider = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// When data changes -> recalculate properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnXYChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _derivative = XYData.Derive();
            _integral = XYData.Integrate();
        }

        /// <summary>
        /// When DataProvider is changed, add data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(DataProvider)) return;
            XYData = new ObservableCollection<DataPoint>(_dataProvider.XYData);
        }

        /// <summary>
        /// <inheritdoc cref="ISpectrum.XYData"/>
        /// </summary>
        public ObservableCollection<DataPoint> XYData { get; set; } = new ObservableCollection<DataPoint>();

        /// <summary>
        /// Backing field for <see cref="Spectrum.Derivative"/>
        /// </summary>
        private IEnumerable<DataPoint> _derivative;

        /// <summary>
        /// Derivative of XYData
        /// </summary>
        public ObservableCollection<DataPoint> Derivative => new ObservableCollection<DataPoint>(_derivative ??= XYData.Derive());

        /// <summary>
        /// Backing field for <see cref="Spectrum.Integral"/>
        /// </summary>
        private IEnumerable<DataPoint> _integral;

        /// <summary>
        /// Integral of XYData
        /// </summary>
        public ObservableCollection<DataPoint> Integral => new ObservableCollection<DataPoint>(_integral ??= XYData.Integrate());

        /// <summary>
        /// <inheritdoc cref="ISpectrum.Title"/>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the Title
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Title;

        /// <summary>
        /// Indexer for Properties
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[string index]
        {
            get
            {
                if (DataProvider is IParameterProvider provider) return provider[index];
                throw new Exception("There is no Parameter Provider added");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
