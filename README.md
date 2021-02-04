![](https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/.github/chemsharp.png)
# Chem# (ChemSharp)

[![Maintainability](https://api.codeclimate.com/v1/badges/bb81db40213cc68deb97/maintainability)](https://codeclimate.com/github/JensKrumsieck/ChemSharp/maintainability)
![.NET Core](https://github.com/JensKrumsieck/ChemSharp/workflows/.NET%20Core/badge.svg)
[![GitHub license](https://img.shields.io/github/license/JensKrumsieck/ChemSharp)](https://github.com/JensKrumsieck/ChemSharp/blob/master/LICENSE)

| | |
|-|-|
| `ChemSharp` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp)](https://www.nuget.org/packages/ChemSharp/) |
| `ChemSharp.Molecules` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Molecules)](https://www.nuget.org/packages/ChemSharp.Molecules/) |
| `ChemSharp.Spectroscopy` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Spectroscopy)](https://www.nuget.org/packages/ChemSharp.Spectroscopy/) |
|`ChemSharp.UnitConversion` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.UnitConversion)](https://www.nuget.org/packages/ChemSharp.UnitConversion/) |
| `ChemSharp.Rendering` | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ChemSharp.Rendering)](https://www.nuget.org/packages/ChemSharp.Rendering/)|

### Features
* Open and process Spectroscopy related files (see Supported Filetypes)
* Open and process Molecular files (see Supported Filetypes)
* Sum formula operations and elemental analysis calculation
* Unit Conversion for (Energy, Magnetic Units, Mass)
* Using Elemental Data from https://github.com/JensKrumsieck/periodic-table and natural constants

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
