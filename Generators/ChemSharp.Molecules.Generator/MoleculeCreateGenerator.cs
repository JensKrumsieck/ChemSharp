using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ChemSharp.Molecules.Generator;

[Generator]
public class MoleculeCreateGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context) { }

	public void Execute(GeneratorExecutionContext context)
	{
		var formats = context.GetClassesImplementing("IAtomFileFormat");
		var sb = new StringBuilder(@"
		using System.IO;
		using ChemSharp.Files;
		using ChemSharp.Molecules.Formats;

		namespace ChemSharp.Molecules{

			public partial class Molecule
			{
				public static Molecule FromFile(string path){
					var mol = ReadFile(path);
					if (mol == null) throw new IOException(""Could not open file! Are you sure it is a supported format?"");
							return mol;
				}
				public static Molecule FromStream(Stream stream, string ext){
					var mol = ReadStream(stream, ext);
					if (mol == null) throw new IOException(""Could not open file! Are you sure it is a supported format?"");
							return mol;
				}

				private static Molecule? ReadFile(string path) => FileHandler.GetExtension(path) switch
				{
		");
		foreach (var format in formats)
		{
			var ext = format.Identifier.ValueText.Split(new[] {"Format"}, StringSplitOptions.RemoveEmptyEntries)[0]
			                .ToLower();
			sb.AppendLine($@"""{ext}"" => {format.Identifier.ValueText}.Read(path),");
		}

		sb.Append(@"
				_ => null
				};

				private static Molecule? ReadStream(Stream stream, string extension) => extension switch
				{
		");
		foreach (var format in formats)
		{
			var ext = format.Identifier.ValueText.Split(new[] {"Format"}, StringSplitOptions.RemoveEmptyEntries)[0]
			                .ToLower();
			sb.AppendLine($@"""{ext}"" => {format.Identifier.ValueText}.ReadFromStream(stream),");
		}

		sb.Append(@"
				_ => null
				};
			}
		}");
		context.AddSource("Molecule_Create", SourceText.From(sb.ToString(), Encoding.UTF8));
	}
}
