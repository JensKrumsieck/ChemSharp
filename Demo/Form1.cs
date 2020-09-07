using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using ChemSharp.Tests.Spectroscopy;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadNMR()
        {
            var model = new PlotModel();
            var dataPoints = CreateDataPoints(BrukerNMRTest.ac, BrukerNMRTest.fid);
            model.Series.Add(new LineSeries() {ItemsSource = dataPoints});
            nmrPV.Model = model;
        }

        private void LoadUVVis()
        {
            var model = new PlotModel();
            var datapoints = SpectrumFactory.Create<UVVisSpectrum>(VarianUVVisTest.path).Data
                .Select(s => new DataPoint(s.X, s.Y));
            model.Series.Add(new LineSeries() {ItemsSource = datapoints});
            uvPV.Model = model;
        }

        private void LoadEPR()
        {
            var model = new PlotModel();
            var datapoints = SpectrumFactory.Create<EPRSpectrum>($"{BrukerEPRTest.path}.par", $"{BrukerEPRTest.path}.spc").Data
                .Select(s => new DataPoint(s.X, s.Y));

            model.Series.Add(new LineSeries() { ItemsSource = datapoints });
            eprPV.Model = model;
        }

        public IEnumerable<DataPoint> CreateDataPoints(IXSpectrumFile xData, IYSpectrumFile yData)
        {
            for(var i = 0; i < xData.XData.Length; i++) yield return new DataPoint(xData.XData[i], yData.YData[i]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadNMR();
            LoadUVVis();
            LoadEPR();
        }
    }
}
