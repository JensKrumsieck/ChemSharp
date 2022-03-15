using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChemSharp.DataProviders;
using ChemSharp.Files;

namespace ChemSharp.Spectroscopy.DataProviders;

public class VarianUVVisProvider : AbstractXYDataProvider
{
    /// <summary>
    /// import recipes
    /// </summary>
    static VarianUVVisProvider()
    {
        if (!FileHandler.RecipeDictionary.ContainsKey("dsw"))
            FileHandler.RecipeDictionary.Add("dsw", s =>
            {
                var file = new PlainFile<float>(s) { ByteOffset = 0x459 };
                file.CutOffLength = BitConverter.ToInt32(file.Bytes, 0x6d) * 8;
                return file;
            });
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="path"></param>
    public VarianUVVisProvider(string path) : base(path) => XYData = HandleData(path).ToArray();

    /// <summary>
    /// Handles UVVis Data
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static IEnumerable<DataPoint> HandleData(string path)
    {
        var file = (PlainFile<float>)FileHandler.Handle(path);
        if (file.Content.Length % 2 != 0) throw new InvalidDataException("X and Y Axis Length mismatch");
        for (var i = 0; i < file.Content.Length; i += 2) yield return new DataPoint(file.Content[i], file.Content[i + 1]);
    }
}
