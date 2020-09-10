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

        public IEnumerable<DataPoint> CreateDataPoints(IXSpectrumFile xData, IYSpectrumFile yData) => xData.XData.Select((t, i) => new DataPoint(t, yData.YData[i]));

        private void Form1_Load(object sender, EventArgs e)
        {
            var pvReference = new[]
            {
                nmrPV, processedPV, uvPV, eprPV
            };
            var data = new IEnumerable<DataPoint>[4];
            data[0] = CreateDataPoints(BrukerNMRTest.ac, BrukerNMRTest.fid);
            data[1] = CreateDataPoints(BrukerNMRTest.ac, BrukerNMRTest.oneR);
            data[2] = SpectrumFactory.Create<UVVisSpectrum>(VarianUVVisTest.path).Data
                .Select(s => new DataPoint(s.X, s.Y));
            data[3] = SpectrumFactory.Create<EPRSpectrum>($"{BrukerEPRTest.path}.par", $"{BrukerEPRTest.path}.spc").Data
                .Select(s => new DataPoint(s.X, s.Y));
            for (var i = 0; i < data.Length; i++)
            {
                var model = new PlotModel();
                model.Series.Add(new LineSeries() {ItemsSource = data[i]});
                pvReference[i].Model = model;
            }
        }
    }
}
