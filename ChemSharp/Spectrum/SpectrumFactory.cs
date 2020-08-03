using ChemSharp.Files;
using System;
using System.Numerics;

namespace ChemSharp.Spectrum
{
    /// <summary>
    /// Factory class to generate Spectrum Objects based on abstract files and type
    /// </summary>
    public static class SpectrumFactory
    {
        public static AbstractSpectrum CreateFromFile<T>(IXYSpectrumFile src) where T : AbstractSpectrum
        {
            var spc = (T) Activator.CreateInstance(typeof(T));
            spc.Data = src.XYData;
            return spc;
        }

        public static AbstractSpectrum CreateFromFiles<T>(IXSpectrumFile xSrc, IYSpectrumFile ySrc) where T : AbstractSpectrum
        {
            Vector2[] data = new Vector2[xSrc.XData.Length];
            for (int i = 0; i < xSrc.XData.Length; i++)
            {
                data[i] = new Vector2(xSrc.XData[i], ySrc.YData[i]);
            }
            var spc = (T)Activator.CreateInstance(typeof(T));
            spc.Data = data;
            return spc;
        }

        public static TResult Create<TResult, TSource>(string path) where TResult : AbstractSpectrum where TSource : IXYSpectrumFile
        {
            var file = (TSource)Activator.CreateInstance(typeof(TSource), path);
            return (TResult)CreateFromFile<TResult>(file);
        }
    }
}
