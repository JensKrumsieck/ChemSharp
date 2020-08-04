using ChemSharp.Files;
using ChemSharp.Files.Spectroscopy;
using System;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    /// <summary>
    /// Factory class to generate Spectrum Objects based on abstract files and type
    /// </summary>
    public static class SpectrumFactory
    {
        public static AbstractSpectrum CreateFromFile<T>(IXYSpectrumFile src)
            where T : AbstractSpectrum
        {
            var spc = (T)Activator.CreateInstance(typeof(T));
            spc.Data = src.XYData;
            spc.Files.Add(((AbstractFile)src).Path);
            //add files
            return spc;
        }

        public static AbstractSpectrum CreateFromFiles<T>(IXSpectrumFile xSrc, IYSpectrumFile ySrc)
            where T : AbstractSpectrum
        {
            var data = new Vector2[xSrc.XData.Length];
            for (var i = 0; i < xSrc.XData.Length; i++)
            {
                data[i] = new Vector2(xSrc.XData[i], ySrc.YData[i]);
            }
            var spc = (T)Activator.CreateInstance(typeof(T));
            spc.Data = data;
            //add files
            spc.Files.Add(((AbstractFile)xSrc).Path);
            spc.Files.Add(((AbstractFile)ySrc).Path);
            return spc;
        }

        public static TResult Create<TResult, TSource>(string path)
            where TResult : AbstractSpectrum
            where TSource : AbstractFile, IXYSpectrumFile
        {
            var file = (TSource)Activator.CreateInstance(typeof(TSource), path);
            return (TResult)CreateFromFile<TResult>(file);
        }

        public static TResult Create<TResult, TSourceX, TSourceY>(string xPath, string yPath)
            where TResult : AbstractSpectrum
            where TSourceX : AbstractFile, IXSpectrumFile
            where TSourceY : AbstractFile, IYSpectrumFile
        {
            var xFile = (TSourceX)Activator.CreateInstance(typeof(TSourceX), xPath);
            var yFile = (TSourceY)Activator.CreateInstance(typeof(TSourceY), yPath);
            return (TResult)CreateFromFiles<TResult>(xFile, yFile);
        }
    }
}
