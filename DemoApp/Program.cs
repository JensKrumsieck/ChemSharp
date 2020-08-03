using System;
using ChemSharp.Files;
using ChemSharp.Spectrum;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = SpectrumFactory.Create<UVVisSpectrum, DSW>(
                @"C:\Users\jenso\PowerFolders\Forschung\UVVis\JK001\JK001F01.DSW");

                
            
            Console.WriteLine("");
        }
    }
}
