using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChemSharp.Extensions;
using ChemSharp.Tests.Spectroscopy;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using OxyPlot;
using OxyPlot.Series;

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
            //var data = fid.YData;
            var oneR = BrukerNMRTest.oneR.YData;
            var oneI = BrukerNMRTest.oneI.YData;
            var dp = new DataPoint[fid.YData.Length/2];
            for (int i = 0; i < fid.YData.Length/2; i++)
            {
                dp[i] = new DataPoint(i,new Complex(oneR[i], oneI[i]).Magnitude);
            }
            model.Series.Add(new LineSeries(){ItemsSource =  dp});
            plotView1.Model = model;
        }
    }
}
