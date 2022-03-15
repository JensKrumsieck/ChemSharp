using System.Collections.Generic;
using System.IO;
using ChemSharp.DataProviders;
using ChemSharp.Files;

namespace ChemSharp.Molecules.DataProviders;

public class AbstractAtomDataProvider : AbstractDataProvider, IAtomDataProvider
{
    public string[] Content { get; set; }

    public IEnumerable<Atom> Atoms { get; set; }

    public AbstractAtomDataProvider(string path) : base(path)
    {
        if (string.IsNullOrEmpty(path)) return;
        var file = (PlainFile<string>)FileHandler.Handle(path);
        Content = file.Content;
    }

    public AbstractAtomDataProvider(Stream stream) : base("")
    {
        using var sr = new StreamReader(stream);
        Content = sr.ReadToEnd().Split('\n');
    }

    public virtual void ReadData() { }
}
