using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Services.Commands.Tools
{
	public static class SyntaxAnaliser
	{

		public static List<string> GetMethods(string code)
		{
			SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
			CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

			var result = new List<string>();

			var namespaceDeclaration = (NamespaceDeclarationSyntax)root.Members[0];
			var classDeclaration = (ClassDeclarationSyntax)namespaceDeclaration.Members[0];

			foreach (var item in classDeclaration.Members )
			{
				result.Add($"{((MethodDeclarationSyntax)item).Identifier}");
			}


			return result;
		}

	}
}
