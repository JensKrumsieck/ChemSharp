using ChemSharp.Extensions;
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
        /// <summary>
        /// Create AbstractSpectrum from Single XY File
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Create<T>(IXYSpectrumFile src)
            where T : AbstractSpectrum
        {
            var spc = (T)Activator.CreateInstance(typeof(T));
            spc.Data = src.XYData;
            spc.Files.Add((IFile)(src));
            spc.OnInit();
            //add files
            return spc;
        }

        /// <summary>
        /// Create AbstractSpectrum from two files (X,Y)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xSrc"></param>
        /// <param name="ySrc"></param>
        /// <returns></returns>
        public static T Create<T>(IXSpectrumFile xSrc, IYSpectrumFile ySrc)
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
            spc.Files.Add((IFile)(xSrc));
            spc.Files.Add((IFile)(ySrc));
            spc.OnInit();
            return spc;
        }


        /// <summary>
        /// Creator Method for Spectra
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TResult Create<TResult>(string path)
            where TResult : AbstractSpectrum
        {
            var file = path.CreateObjectFromFile("Spectroscopy");
            return (TResult)Create<TResult>(file as IXYSpectrumFile);
        }

        /// <summary>
        /// Creator Method for Spectra
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSourceX"></typeparam>
        /// <typeparam name="TSourceY"></typeparam>
        /// <param name="xPath"></param>
        /// <param name="yPath"></param>
        /// <returns></returns>
        public static TResult Create<TResult>(string xPath, string yPath)
            where TResult : AbstractSpectrum
        {
            var xFile = xPath.CreateObjectFromFile("Spectroscopy");
            var yFile = yPath.CreateObjectFromFile("Spectroscopy");
            return (TResult)Create<TResult>(xFile as IXSpectrumFile, yFile as IYSpectrumFile);
        }
    }
}
