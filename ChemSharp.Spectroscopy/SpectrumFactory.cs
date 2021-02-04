using ChemSharp.DataProviders;
using ChemSharp.Spectroscopy.DataProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace ChemSharp.Spectroscopy
{
    public static class SpectrumFactory
    {
        /// <summary>
        /// Contains a file extension - type relationship
        /// </summary>
        public static Dictionary<string, Type> DataProviderDictionary = new Dictionary<string, Type>();

        /// <summary>
        /// Import supported types for auto creation
        /// </summary>
        static SpectrumFactory()
        {
            DataProviderDictionary.Add("par", typeof(BrukerEPRProvider));
            DataProviderDictionary.Add("spc", typeof(BrukerEPRProvider));
            DataProviderDictionary.Add("dsw", typeof(VarianUVVisProvider));
            DataProviderDictionary.Add("fid", typeof(BrukerNMRProvider));
            DataProviderDictionary.Add("1r", typeof(BrukerNMRProvider));
            DataProviderDictionary.Add("1i", typeof(BrukerNMRProvider));
            DataProviderDictionary.Add("acqus", typeof(BrukerNMRProvider));
            DataProviderDictionary.Add("procs", typeof(BrukerNMRProvider));
        }

        /// <summary>
        /// Creates a Spectrum object with auto type detection
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Spectrum Create(string path) => new Spectrum((IXYDataProvider)CreateProvider(path));

        /// <summary>
        /// Handles IDataProvider creation
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AbstractDataProvider CreateProvider(string path)
        {
            var ext = GetExtension(path);
            //check if ext is supported
            if (!DataProviderDictionary.ContainsKey(ext)) throw new FileLoadException($"File type {ext} is not supported");
            var type = DataProviderDictionary[ext];
            var param = Expression.Parameter(typeof(string), "path");
            var creator = Expression
                .Lambda<Func<string, AbstractXYDataProvider>>(
                    Expression.New(
                        type.GetConstructor(new[] { typeof(string) }) ??
                        throw new InvalidOperationException("null given, Extension Handling failed"), param), param)
                .Compile();
            return creator(path);
        }

        /// <summary>
        /// Get Extension as string
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            var ext = Path.GetExtension(path);
            //fallback for nmr files
            ext = string.IsNullOrEmpty(ext) ? Path.GetFileName(path) : ext.Remove(0, 1);
            return ext.ToLower();
        }
    }
}
