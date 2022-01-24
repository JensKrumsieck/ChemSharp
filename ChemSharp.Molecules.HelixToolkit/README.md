<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp.Molecules.HelixToolkit</h1> 

[![NuGet Badge](https://buildstats.info/nuget/ChemSharp.Molecules.HelixToolkit?includePreReleases=true)](https://www.nuget.org/packages/ChemSharp.Molecules.HelixToolkit/)

HelixToolkit Bindings for ChemSharp.Molecules (WPF)

Add the necessary xmlns to your .xaml file and create a Viewport3D Element:
```xaml
<Window
xmlns:h="http://helix-toolkit.org/wpf" 
xmlns:controls="clr-namespace:ChemSharp.Molecules.HelixToolkit.Controls;assembly=ChemSharp.Molecules.HelixToolkit"
>
<...>
<h:HelixViewport3D x:Name="Viewport3D"  
                   ShowViewCube="False" ShowCoordinateSystem="True" 
                   ZoomExtentsWhenLoaded="True" IsHeadLightEnabled="True">
    <h:DefaultLights/>
    <h:DirectionalHeadLight/>
    <controls:ItemsVisual3D ItemsSource="{Binding Atoms3D}"/>
    <controls:ItemsVisual3D ItemsSource="{Binding Bonds3D}"/>
</h:HelixViewport3D>
```
This is how an example code-behind file could look like: 
```csharp
public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var filename = @"F:\Repositories\ChemSharp\ChemSharp.Tests\files\ptcor.mol2";
            Molecule = MoleculeFactory.Create(filename);
            Atoms3D = new ObservableCollection<Atom3D>(Molecule.Atoms.Select(s => new Atom3D(s)));
            Bonds3D = new ObservableCollection<Bond3D>(Molecule.Bonds.Select(s => new Bond3D(s)));
        }

        public Molecule Molecule { get; }

        /// <summary>
        /// 3D Representation of Atoms
        /// </summary>
        public ObservableCollection<Atom3D> Atoms3D { get; }
        /// <summary>
        /// 3D Representation of Bonds
        /// </summary>
        public ObservableCollection<Bond3D> Bonds3D { get; }
    }
  ```
  
## Supported Files:
* **Import** (XYZ, CIF (crystallographic information file, CCDC), MOL2 (TRIPOS Mol2), PDB (Protein Data Bank file), CDXML (Single Molecule only))
* **Export** (XYZ, MOL2)
  
<hr/>

#### Used Libraries:
* [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics)

#### Compatibility
* .NET 6.0, .NET5.0
