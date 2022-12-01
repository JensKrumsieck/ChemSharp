using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ChemSharp.Molecules.Generator;

[Generator]
public class FileFormatReadGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context) { }

	public void Execute(GeneratorExecutionContext context)
	{
		var formats = context.GetClassesImplementing("IAtomFileFormat");

		foreach (var format in formats)
		{
			var source = @$"
			using System.IO;
			using static System.IO.Path;
			namespace ChemSharp.Molecules.Formats{{
			public partial class {format.Identifier.ValueText}{{
				public static Molecule Read(string path){{
					var format = new {format.Identifier.ValueText}(path);
					format.ReadFromFileInternal();
					return format.Create();
				}}
				public static Molecule ReadFromStream(Stream stream){{
					var format = new {format.Identifier.ValueText}(string.Empty);
					format.ReadFromStreamInternal(stream);
					return format.Create();
				}}
				private Molecule Create(){{
					var molecule = new Molecule(Atoms{(format.HasInterface("IBondFileFormat") ? ", Bonds" : string.Empty)}) {{Title = GetFileNameWithoutExtension(Path)}};
					if(molecule.Bonds.Count == 0) molecule.RecalculateBonds();
					return molecule;
				}}
			}}
			}}";
			context.AddSource($"{format.Identifier.ValueText}_Read", SourceText.From(source, Encoding.UTF8));
		}
	}
}
