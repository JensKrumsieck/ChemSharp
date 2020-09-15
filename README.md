![](https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/assets/logo.png)
# Chem# (ChemSharp)

[![Maintainability](https://api.codeclimate.com/v1/badges/bb81db40213cc68deb97/maintainability)](https://codeclimate.com/github/JensKrumsieck/ChemSharp/maintainability)
![.NET Core](https://github.com/JensKrumsieck/ChemSharp/workflows/.NET%20Core/badge.svg)

### Features
* Open and process Spectroscopy related files (see Supported Filetypes)
* Open and process Molecular files (see Supported Filetypes)
* Sum formula operations and elemental analysis calculation
* Unit Conversion for 
	* Energy
	* Magnetic Units
	* Mass
* Using Elemental Data from https://github.com/JensKrumsieck/periodic-table and natural constants

### Supported Filetypes
* Molecule
	* XYZ
	* CIF (crystallographic information file)
	* MOL2 (TRIPOS Mol2)

* Spectroscopy
	* Varian/Agilient DSW
	* Bruker EMX SPC/PAR
	* Bruker TopSpin (fid, (1r/1i processed spectra), JCAMP-DX (acqus, procs, ...))
	* CSV

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)