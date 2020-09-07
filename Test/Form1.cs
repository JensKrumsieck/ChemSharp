using ChemSharp.Extensions;
using ChemSharp.Tests.Spectroscopy;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Linq;
using System.Windows.Forms;
using Fourier = ChemSharp.Extensions.Fourier;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var model = new PlotModel();
            var fid = BrukerNMRTest.fid;
            var xs = BrukerNMRTest.ac.XData;
            var ft = fid.YData;
            var fidd = new DataPoint[fid.YData.Length];
            for (var i = 0; i < fid.YData.Length; i++)
            {
                fidd[i] = new DataPoint(xs.Select(s => (double)s).ElementAt(i), ft[i]);
            }
            model.Series.Add(new LineSeries() { ItemsSource = fidd });
            var axis = new LinearAxis()
            {
                Position = AxisPosition.Bottom
            };
            model.Axes.Add(axis);
            plotView1.Model = model;
        }
    }
}
