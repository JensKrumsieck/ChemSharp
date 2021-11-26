<p align="center">
<img src="https://raw.githubusercontent.com/JensKrumsieck/ChemSharp/master/icon.png" height="125px" /></p>
<h1 align="center" >ChemSharp.UnitConversion</h1>

[![NuGet Badge](https://buildstats.info/nuget/ChemSharp.UnitConversion?includePreReleases=true)](https://www.nuget.org/packages/ChemSharp.UnitConversion/)

### Features
* Unit Conversion for (Energy, Magnetic Units, Mass)

### Basic Usage

Use given Converter:
```csharp
//create converter instance (energy)
var converter = new EnergyUnitConverter("nm", "cm^-1");
//convert using Convert or ConvertInverted
var wavenumbers = converter.Convert(500) / 1000;
var nanometers = converter.ConvertInverted(50000) * 1000
```

Create own Converter [EnergyUnitConverter](https://github.com/JensKrumsieck/ChemSharp/blob/master/ChemSharp.UnitConversion/EnergyUnitConverter.cs):
```csharp
namespace ChemSharp.UnitConversion
{
    public class EnergyUnitConverter : AbstractUnitConverter
    {
        public EnergyUnitConverter(string from, string to) : base(from, to)
        {
            AddConversion("J", 1);
            AddConversion("eV", Constants.ElectronCharge);
            AddConversion("kJ/mol", 1000 / Constants.Avogadro);
            AddConversion("cal", 4.184);
            AddConversion("kcal/mol", 4184 / Constants.Avogadro);
            AddConversion("hartree", Constants.Hartree);
            AddConversion("cm^-1", Constants.Planck * Constants.SpeedOfLight * 100);
            AddConversion("Hz", Constants.Planck);
            AddConversion("Kelvin", Constants.Boltzmann);
            AddConversion("nm", new MappedFunction()
            {
                Function = i => Constants.Planck * Constants.SpeedOfLight / (i * 1e-9),
                Reciprocal = true
            });
        }
    }
}
```

#### Compatibility
* .NET Standard 2.0, .NET Standard 2.1, .NET 5, .NET 6
