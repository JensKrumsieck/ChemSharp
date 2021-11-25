<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp.Molecules</h1>

Package for processing of molecule related files.

### Features
* Open and process Molecular files (see [Supported Filetypes](#molecule))
* Sum formula operations and elemental analysis calculation
* Using Elemental Data from https://github.com/JensKrumsieck/periodic-table


## Basic Usage
#### Create Molecules
Molecules can be created in a lot of ways. The easiest way is to use MoleculeFactory.Create, which accepts a string path. Depending on the File extension the correct DataProvider is used to load the file.
  ```csharp
  //Creates a molecule from cif file
  const string path = "files/cif.cif";
  var mol = MoleculeFactory.Create(path);
  ```
It is also possible to create a Molecule by using a specific DataProvider (e.g. if automatic detection fails or you only want to support a selected number of file types)

  ```csharp
  //You can also create molecules by selecting the provider yourself
  const string path = "files/benzene.mol2";
  var provider = new Mol2DataProvider(path);
  var mol = new Molecule(provider);
  ```
You can also add [Atoms and Bonds](https://github.com/JensKrumsieck/ChemSharp/wiki/Element-Atom-Bond) as Lists if you got the data from somewhere else.

  ```csharp
  //...or by just adding the Atoms & Bonds as Lists
  const string path = "files/cif.cif";
  var provider = new CIFDataProvider(path);
  var mol = new Molecule(provider.Atoms, provider.Bonds);
  ```

## Supported Files:
* **Import** (XYZ, CIF (crystallographic information file, CCDC), MOL2 (TRIPOS Mol2), PDB (Protein Data Bank file), CDXML (Single Molecule only))
* **Export** (XYZ, MOL2)
  
<hr/>

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility
* .NET Standard 2.0, .NET Standard 2.1, .NET 5, .NET 6
* Unity (see [Wiki](https://github.com/JensKrumsieck/ChemSharp/wiki/Use-with-Unity) 
<a href="https://github.com/JensKrumsieck/ChemSharp/wiki/Use-with-Unity"><img src="https://img.shields.io/badge/Unity-100000?logo=unity&logoColor=white"/></a>)
* Godot Engine (see [Wiki](https://github.com/JensKrumsieck/ChemSharp/wiki/Use-with-Godot-Engine) for Snippet)
* Blazor (see ChemSharp.Molecules.Blazor)
