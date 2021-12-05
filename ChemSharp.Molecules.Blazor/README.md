<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp.Molecules.Blazor</h1> 

[![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Molecules.Blazor?includePreReleases=true)](https://www.nuget.org/packages/ChemSharp.Molecules.Blazor/)

Blazor view for ChemSharp.Molecules using Three.js

Creates a BlazorMolecule-Component to be used in your razor-Views. Supports all files supported by ChemSharp.Molecules.
You can use BlazorMoleculeFactory to create molecule from ```IBrowserFile```

Example usage of BlazorMolecule component (razor-file):
  ```razor
@page "/molecule"
@using ChemSharp.Molecules.Blazor
@inject IJSRuntime JS

<h3>Molecule</h3>
<InputFile OnChange="OnFileChange" id="filedrop" class="form-control" />   
<BlazorMolecule Molecule=molecule class="lol" style="height: 50vh;"/>

@code {

    Molecule? molecule;

    private async Task OnFileChange(InputFileChangeEventArgs args)
    {
        var file = args.File;
        molecule = await BlazorMoleculeFactory.CreateAsync(file);
    }
}
  ```
 Example Project:
 https://github.com/JensKrumsieck/blazor-playground
  
## Supported Files:
* **Import** (XYZ, CIF (crystallographic information file, CCDC), MOL2 (TRIPOS Mol2), PDB (Protein Data Bank file), CDXML (Single Molecule only))
* **Export** (XYZ, MOL2)
  
<hr/>

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility
* .NET 6.0
