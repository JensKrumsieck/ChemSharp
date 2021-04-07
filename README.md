![](https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/.github/chemsharp.png)
# Chem# (ChemSharp)

[![Maintainability](https://api.codeclimate.com/v1/badges/bb81db40213cc68deb97/maintainability)](https://codeclimate.com/github/JensKrumsieck/ChemSharp/maintainability)
![.NET](https://github.com/JensKrumsieck/ChemSharp/workflows/.NET/badge.svg)
[![GitHub license](https://img.shields.io/github/license/JensKrumsieck/ChemSharp)](https://github.com/JensKrumsieck/ChemSharp/blob/master/LICENSE)
[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.4573532.svg)](https://doi.org/10.5281/zenodo.4573532)

| | |
|-|-|
| `ChemSharp` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp)](https://www.nuget.org/packages/ChemSharp/) |
| `ChemSharp.Molecules` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Molecules)](https://www.nuget.org/packages/ChemSharp.Molecules/) |
| `ChemSharp.Spectroscopy` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Spectroscopy)](https://www.nuget.org/packages/ChemSharp.Spectroscopy/) |
|`ChemSharp.UnitConversion` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.UnitConversion)](https://www.nuget.org/packages/ChemSharp.UnitConversion/) |
| `ChemSharp.Rendering` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Rendering)](https://www.nuget.org/packages/ChemSharp.Rendering/)|

### Features
* Open and process Spectroscopy related files (see [Supported Filetypes](#spectroscopy))
* Open and process Molecular files (see [Supported Filetypes](#molecule))
* Sum formula operations and elemental analysis calculation
* Unit Conversion for (Energy, Magnetic Units, Mass)
* Using Elemental Data from https://github.com/JensKrumsieck/periodic-table and natural constants

### Basic Usage (See [Wiki](https://github.com/JensKrumsieck/ChemSharp/wiki))
#### Create Spectra
```csharp
//Creates an UV/Vis Spectrum
const string path = "files/uvvis.dsw";
var uvvis = SpectrumFactory.Create(path);

//You can also create spectra by choosing the provider 
//explicitly. e.g. csv files
//Reads in an CSV Spectrum (first data only)
const string path = "files/uvvis.csv";
var prov = new GenericCSVProvider(path);
var uvvis = new Spectrum(prov);

//To read in all CSV Data stored as (X,Y) pairs use the MultiCSVProvider
//Each Spectrum will be stored as DataPoint[] in MultiXYData
const string file = "files/multicsv.csv";
var provider = new MultiCSVProvider(file);
```
#### Create Molecules
```csharp
//Creates a molecule from cif file
const string path = "files/cif.cif";
var mol = MoleculeFactory.Create(path);

//You can also create molecules by selecting the provider yourself
const string path = "files/benzene.mol2";
var provider = new Mol2DataProvider(path);
var mol = new Molecule(provider);

//...or by just adding the Atoms & Bonds as Lists
const string path = "files/cif.cif";
var provider = new CIFDataProvider(path);
var mol = new Molecule(provider.Atoms, provider.Bonds);
```
### Supported Filetypes
* ### Molecule
	* #### Import
		* XYZ
		* CIF (crystallographic information file)
		* MOL2 (TRIPOS Mol2)
		* CDXML (Single Molecule only)
	* #### Export
		* XYZ
		* MOL2
		* SVG

* ### Spectroscopy
	* #### Import
		* Varian/Agilient DSW
		* Bruker EMX SPC/PAR
		* Bruker TopSpin (fid, (1r/1i processed spectra), JCAMP-DX (acqus, procs, ...))
		* CSV
	* #### Export
		* CSV

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility
* NET Standard 2.0 (tested with NET Framework 4.7.2 & NET Core 2.1, see Unit Tests)
* NET Standard 2.1 (tested with NET 5.0, see Unit Tests)

### How to cite
You can either cite the package with via the DOI: [10.5281/zenodo.4573532](https://doi.org/10.5281/zenodo.4573532) (universal DOI, there is also one for each version if you want to be specific about that. Just click the link :smirk:)

### Used by (Highlights):
*  <img src="https://github.com/JensKrumsieck/PorphyStruct/blob/master/PorphyStruct.WPF/Resources/porphystruct.png" alt="logo" height="16"/>  **[PorphyStruct](https://github.com/JensKrumsieck/PorphyStruct)**
* <img src="https://raw.githubusercontent.com/JensKrumsieck/SPCViewer/master/.github/spc.png" alt="logo" height="16"/>  **[SPCViewer](https://github.com/JensKrumsieck/SPCViewer)**
* * <img src="https://raw.githubusercontent.com/JensKrumsieck/CHN-Tool/master/.github/chn.png" alt="logo" height="16"/>  **[CHN-Tool](https://github.com/JensKrumsieck/CHN-Tool)**

