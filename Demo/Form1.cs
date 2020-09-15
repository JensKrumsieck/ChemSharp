using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using ChemSharp.Tests.Spectroscopy;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChemSharp.Molecule;

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
                model.Series.Add(new LineSeries() { ItemsSource = data[i] });
                pvReference[i].Model = model;
            }

            ElementsTB.Font = new Font(new FontFamily("Arial"), 14, FontStyle.Regular);
            PaintElements();
        }

        private void PaintElements()
        {
            for(var i = 0; i < 118; i++)
            {
                var element = ElementDataProvider.Elements.ElementAt(i);
                AppendText(ElementsTB, element.Name + "(" + element.Symbol + ")", ColorTranslator.FromHtml(element.Color));
            }
        }

        private static void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
