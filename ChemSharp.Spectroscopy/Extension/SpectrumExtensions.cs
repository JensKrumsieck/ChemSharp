using ChemSharp.Extensions;
using ChemSharp.Spectroscopy.DataProviders;
using System;
using System.IO;

namespace ChemSharp.Spectroscopy.Extension
{
    public static class SpectrumExtensions
    {
        private static readonly DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();

        /// <summary>
        /// Returns default Unit as String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Unit(this Spectrum input) =>
            input.DataProvider switch
            {
                BrukerEPRProvider epr => epr["JUN"],
                BrukerNMRProvider => "ppm",
                VarianUVVisProvider => "nm",
                GenericCSVProvider csv => csv["Unit"],
                _ => ""
            };

        /// <summary>
        /// Returns default Quantity for X Axis as String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Quantity(this Spectrum input) =>
            input.DataProvider switch
            {
                BrukerEPRProvider => "B",
                BrukerNMRProvider => "δ",
                VarianUVVisProvider => "λ",
                GenericCSVProvider csv => csv["XTitle"],
                _ => ""
            };

        /// <summary>
        /// Returns default Quantity for Y Axis as String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string YQuantity(this Spectrum input) =>
            input.DataProvider switch
            {
                BrukerEPRProvider => "a.u.",
                BrukerNMRProvider => "a.u.",
                VarianUVVisProvider => "rel. Abs.",
                GenericCSVProvider csv => csv["YTitle"],
                _ => ""
            };

        /// <summary>
        /// Returns the Creation Date of Spectrum
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime CreationDate(this Spectrum input) =>
            input.DataProvider switch
            {
                BrukerEPRProvider => DateTime.Parse($"{input["JDA"]} {input["JTM"]}"),
                BrukerNMRProvider => UNIX.AddSeconds(input["##$DATE"].ToInt()),
                _ => File.GetCreationTime(input.Title)
            };
    }
}
