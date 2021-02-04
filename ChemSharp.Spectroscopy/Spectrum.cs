using ChemSharp.DataProviders;
using ChemSharp.Extensions;
using ChemSharp.Spectroscopy.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Spectroscopy
{
    public class Spectrum : ISpectrum, IDataObject
    {
        private IXYDataProvider _dataProvider;
        ///<summary>
        /// <inheritdoc />
        /// </summary>
        public IXYDataProvider DataProvider
        {
            get => _dataProvider;
            set
            {
                _dataProvider = value;
                DataProviderChanged();
            }
        }

        /// <summary>
        /// When DataProvider is changed, add data
        /// </summary>
        private void DataProviderChanged()
        {
            XYData = _dataProvider.XYData.ToList();
            Title = _dataProvider.Path;
        }

        /// <summary>
        /// <inheritdoc cref="ISpectrum.XYData"/>
        /// </summary>
        public List<DataPoint> XYData { get; set; } = new List<DataPoint>();

        /// <summary>
        /// Backing field for <see cref="Spectrum.Derivative"/>
        /// </summary>
        private IEnumerable<DataPoint> _derivative;

        /// <summary>
        /// Derivative of XYData
        /// </summary>
        public List<DataPoint> Derivative => (_derivative ??= XYData.Derive()).ToList();

        /// <summary>
        /// Backing field for <see cref="Spectrum.Integral"/>
        /// </summary>
        private IEnumerable<DataPoint> _integral;

        /// <summary>
        /// Integral of XYData
        /// </summary>
        public List<DataPoint> Integral => (_integral ??= XYData.Integrate()).ToList();

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

        public DateTime CreationDate => this.CreationDate();
    }
}
