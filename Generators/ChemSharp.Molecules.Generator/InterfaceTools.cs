using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ChemSharp.Molecules.Generator;

public static class InterfaceTools
{
	public static bool HasInterface(this ClassDeclarationSyntax syn, string interfaceName)
	{
		var basetypes = syn.BaseList.Types.Select(s => s);
		return basetypes.Any(s => s.ToString() == interfaceName);
	}

	public static IEnumerable<ClassDeclarationSyntax> GetClassesImplementing(
		this GeneratorExecutionContext context, string interfaceName) =>
		context.Compilation.SyntaxTrees
		       .SelectMany(tree => tree.GetRoot().DescendantNodes())
		       .Where(x => x is ClassDeclarationSyntax)
		       .Cast<ClassDeclarationSyntax>().Where(s => s.BaseList is not null)
		       .Where(s => s.HasInterface(interfaceName));
}
