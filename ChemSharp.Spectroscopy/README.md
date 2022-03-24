<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp.Spectroscopy</h1>

[![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Spectroscopy?includePreReleases=true)](https://www.nuget.org/packages/ChemSharp.Spectroscopy/)

### Features

* Open and process Spectroscopy related files (see [Supported Filetypes](#supported-filetypes))

#### Create Spectra

Spectra can be created in a lot of ways. The easiest way is to use `SpectrumFactory.Create`, which accepts
a `string path`. Depending on the File extension the correct DataProvider is used to load the file.

```csharp
//Creates an UV/Vis Spectrum
const string path = "files/uvvis.dsw";
var uvvis = SpectrumFactory.Create(path);
```

It is also possible to create a Spectrum by using a specific DataProvider (e.g. if automatic detection fails or you only
want to support a selected number of file types)

```csharp
//You can also create spectra by choosing the provider 
//explicitly. e.g. csv files
//Reads in an CSV Spectrum (first data only)
const string path = "files/uvvis.csv";
var prov = new GenericCSVProvider(path);
var uvvis = new Spectrum(prov);
```

There is also the MultiCSVProvider which can provide data from multiple XY pairs in a csv file

```csharp
//To read in all CSV Data stored as (X,Y) pairs use the MultiCSVProvider
//Each Spectrum will be stored as DataPoint[] in MultiXYData
const string file = "files/multicsv.csv";
var provider = new MultiCSVProvider(file);
```

### Supported Filetypes

* **Import** (Varian/Agilient DSW, Bruker EMX SPC/PAR, Bruker TopSpin (fid, (1r/1i processed spectra), JCAMP-DX (acqus,
  procs, ...)), CSV)
* **Export** (CSV)

#### Used Libraries:

* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility

* .NET Standard 2.0, .NET Standard 2.1, .NET 5, .NET 6
