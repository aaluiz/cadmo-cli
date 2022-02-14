using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Contracts;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tests
{
    public static class CSharpCompiler
    {
        public static bool ValidateSourceCode(string code)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(Models.ClassElements).GetTypeInfo().Assembly.Location,
                typeof(Tools.Class1).GetTypeInfo().Assembly.Location,
                typeof(Services.ClassDefinition).GetTypeInfo().Assembly.Location,
                typeof(Contracts.Abstractions.ClassDefintionAbstraction).GetTypeInfo().Assembly.Location,
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location)!, "System.Runtime.dll"),
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            // Write("Adding the following references");
            // foreach (var r in refPaths)
            // 	Write(r);

            Write("Compiling ...");
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success) WriteErrorMessages(result);
                return result.Success;
            }
        }

        private static void WriteErrorMessages(EmitResult result)
        {
            if (!result.Success)
            {
                Write("Compilation failed!");
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
        }

        private static void Write(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}