<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp</h1>
<h3 align="center">.NET Library for processing of chemistry related files. Powers <a href="https://github.com/JensKrumsieck/PorphyStruct">PorphyStruct</a>!</h3>

[![Maintainability](https://api.codeclimate.com/v1/badges/bb81db40213cc68deb97/maintainability)](https://codeclimate.com/github/JensKrumsieck/ChemSharp/maintainability)
![.NET](https://github.com/JensKrumsieck/ChemSharp/workflows/.NET/badge.svg)
[![GitHub issues](https://img.shields.io/github/issues/JensKrumsieck/ChemSharp)](https://github.com/JensKrumsieck/ChemSharp/issues)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/JensKrumsieck/ChemSharp)
[![GitHub license](https://img.shields.io/github/license/JensKrumsieck/ChemSharp)](https://github.com/JensKrumsieck/ChemSharp/blob/master/LICENSE)
![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/jenskrumsieck/chemsharp)
[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.4573532.svg)](https://doi.org/10.5281/zenodo.4573532)

<hr/>

### NuGet Packages

| | |
|-|-|
| `ChemSharp` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp)](https://www.nuget.org/packages/ChemSharp/) |
| `ChemSharp.Molecules` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Molecules)](https://www.nuget.org/packages/ChemSharp.Molecules/) |
| `ChemSharp.Molecules.Blazor` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Molecules.Blazor)](https://www.nuget.org/packages/ChemSharp.Molecules.Blazor/) |
| `ChemSharp.Molecules.HelixToolkit` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Molecules.HelixToolkit)](https://www.nuget.org/packages/ChemSharp.Molecules.HelixToolkit/) |
| `ChemSharp.Spectroscopy` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Spectroscopy)](https://www.nuget.org/packages/ChemSharp.Spectroscopy/) |
|`ChemSharp.UnitConversion` | [![NuGet Badge](https://buildstats.info/nuget/ChemSharp.UnitConversion)](https://www.nuget.org/packages/ChemSharp.UnitConversion/) |


<sup>CI Releases at: <a href="https://github.com/JensKrumsieck/ChemSharp/packages/">GitHub Packages</a> </sup>

### Features

* Open and process Molecular files (see [Supported Filetypes](#molecule))
* Sum formula operations and elemental analysis calculation
* Using Elemental Data from https://github.com/JensKrumsieck/periodic-table
* Blazor view for molecules using three.js
* Open and process Spectroscopy related files (see [Supported Filetypes](#spectroscopy))
* Unit Conversion for (Energy, Magnetic Units, Mass)

### Basic Usage

#### Create Molecules

Molecules can be created in a lot of ways. The easiest way is to use MoleculeFactory.Create, which accepts a string
path.

```csharp
//Creates a molecule from cif file
const string path = "files/cif.cif";
var mol = Molecule.FromFile(path);
```

#### Create Spectra

Spectra can be created by using the Spectrum.FromFile Method.

```csharp
//Creates an UV/Vis Spectrum
const string path = "files/uvvis.dsw";
var uvvis = Spectrum.FromFile(path);
```


### Supported Filetypes

* **Molecule**
    * **Import** (**XYZ**, **CIF** (crystallographic information file, CCDC), **MOL2** (TRIPOS Mol2), **PDB** (Protein
      Data Bank file), **MOL** (MDL MOL, ChemSpider), **CDXML** (Single Molecule only, 2D))
    * **Export** (XYZ, MOL2)

* **Spectroscopy**
    * **Import** (Varian/Agilient DSW, Bruker EMX SPC/PAR, Bruker TopSpin (fid, (1r/1i processed spectra), JCAMP-DX (
      acqus, procs, ...)), CSV)
    * **Export** (CSV)

#### Used Libraries:

* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility

* .NET Standard 2.0, .NET Standard 2.1, .NET 5, .NET 6
* Blazor (see ChemSharp.Molecules.Blazor)
* <a href="https://github.com/JensKrumsieck/ChemSharp/wiki/Use-with-HelixToolkit-(WPF)">HelixToolkit</a> via
  ChemSharp.Molecules.HelixToolkit
* Unity (see https://github.com/JensKrumsieck/ChemSharpUnityExample)

### Used by (Highlights):

*  <img src="https://github.com/JensKrumsieck/PorphyStruct/blob/master/PorphyStruct.WPF/Resources/porphystruct.png" alt="logo" height="16"/>  **[PorphyStruct](https://github.com/JensKrumsieck/PorphyStruct)**
* <img src="https://raw.githubusercontent.com/JensKrumsieck/CHN-Tool/master/.github/chn.png" alt="logo" height="16"/>  **[CHN-Tool](https://github.com/JensKrumsieck/CHN-Tool)**

### Stats

![Alt](https://repobeats.axiom.co/api/embed/dc542332761cc7e16b22d8bfe0454a55de4620c4.svg "Repobeats analytics image")
