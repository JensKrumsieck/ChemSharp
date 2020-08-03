using System;
using ChemSharp.Files;
using ChemSharp.Spectrum;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var uv = SpectrumFactory.Create<UVVisSpectrum, DSW>(
                @"C:\Users\jenso\PowerFolders\Forschung\UVVis\JK001\JK001F01.DSW");
            var path = @"C:\Users\jenso\PowerFolders\Forschung\EPR\DipyOMeIPCoCl\8K dcm\Spectrum33";
            var epr = SpectrumFactory.Create<EPRSpectrum, PAR, SPC>($"{path}.par", $"{path}.spc");
            

            Console.WriteLine("");
        }
    }
}
