![](https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/assets/logo.png)
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

### Features
* [x] Open and process Spectroscopy related files (see Supported Filetypes)
* [x] Open and process Molecular files (see Supported Filetypes)
* [x] Sum formula operations and elemental analysis calculation
* [x] Unit Conversion for 
	* [x] Energy
	* [x] Magnetic Units
	* [x] Mass
* [x] Using Elemental Data from https://github.com/JensKrumsieck/periodic-table and natural constants

### Supported Filetypes
* [x] Molecule
	* [x] XYZ
	* [x] CIF (crystallographic information file)
	* [x] MOL2 (TRIPOS Mol2)

* [x] Spectroscopy
	* [x] Varian/Agilient DSW
	* [x] Bruker EMX SPC/PAR
	* [x] Bruker TopSpin (fid, (1r/1i processed spectra), JCAMP-DX (acqus, procs, ...))
	* [x] CSV

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)
