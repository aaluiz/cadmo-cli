
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceGenerator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            const string attribute = @"// <auto-generated />
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class AddServiceAttribute : System.Attribute
{
}
";
            context.RegisterForPostInitialization(context => context.AddSource("AddService.Generated.cs", SourceText.From(attribute, Encoding.UTF8)));
            context.RegisterForSyntaxNotifications(() => new ServicesReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (ServicesReceiver?)context.SyntaxReceiver;

            if (receiver == null) return;

            if (!ValidateBySyntaxReceiver(receiver)) return;

            var registrations = ServiceRegistration(receiver.ClassesToRegister.ToImmutableList(), context);

            if (RegistrationsIsNull(registrations)) return;

            string? code = GenerateExtensionAddServicesToDI(receiver, context, registrations);

            if (code == null) return;

            context.AddSource("GeneratedServicesExtension.Generated.cs", SourceText.From(code, Encoding.UTF8));
        }

        private string? GenerateExtensionAddServicesToDI(ServicesReceiver? receiver, GeneratorExecutionContext context, StringBuilder? registrations)
        {
            if (receiver == null) return null;
            // var invocationSemanticModel = context.Compilation.GetSemanticModel(receiver.InvocationSyntaxNode.SyntaxTree);
            // var methodSyntax = receiver.InvocationSyntaxNode.FirstAncestorOrSelf<MethodDeclarationSyntax>();
            // if (methodSyntax == null)
            //	return null;
            //var methodSymbol = invocationSemanticModel.GetDeclaredSymbol(methodSyntax);
            // if (methodSymbol == null)
            //return null;
            var code = $@"    public static class GeneratedServicesExtension
    {{
        public static void AddServicesToDI(this IServiceCollection services)
        {{
{registrations}        }}
    }}";

            code = NamespaceAnalisysFormat("Services", code);

            code = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;
using Services.Coder;
using Services.Startup;
using Contracts.Interfaces;

" + code;

            return code;
        }

        private static string NamespaceAnalisysFormat(string nameSpace, string code)
        {
            //var ns = nameSpace;
            //var ns = methodSymbol.ContainingNamespace.Name.ToString();
            code = $@"namespace {nameSpace}
{{
{code}
}}
";

            return code;
        }

        private static string ProcessIdentation(string code)
        {
            var newClassCodeBuilder = new StringBuilder();
            foreach (var line in code.Split(new[] { @"
" }, StringSplitOptions.None))
            {
                if (line.Length > 4 && line.Substring(0, 4) == "    ")
                    newClassCodeBuilder.AppendLine(line.Substring(4, line.Length - 4));
                else
                    newClassCodeBuilder.AppendLine(line);
            }
            code = newClassCodeBuilder.ToString();
            return code;
        }

        private bool ValidateBySyntaxReceiver(ServicesReceiver? receiver)
        {
            if (receiver == null || !receiver.ClassesToRegister.Any())
                return false;

            return true;
        }

        private bool SemanticModelIsNull(SemanticModel semanticModel)
        {
            return semanticModel == null ? true : false;
        }

        private bool AddServiceAttributeExists(ISymbol? symbol)
        {
            if (symbol == null) return false;
            return !symbol.GetAttributes().Any(a => a.AttributeClass?.Name == "AddServiceAttribute");
        }

        private StringBuilder? ServiceRegistration(ImmutableList<ClassDeclarationSyntax> classes, GeneratorExecutionContext context)
        {
            var result = new StringBuilder();
            const string spaces = "            ";

            foreach (var item in classes)
            {
                var semanticModel = context.Compilation.GetSemanticModel(item.SyntaxTree);

                if (SemanticModelIsNull(semanticModel)) continue;

                var symbol = semanticModel.GetDeclaredSymbol(item);

                if (symbol == null) return null;

                if (AddServiceAttributeExists(symbol)) continue;

                result.Append(spaces);
                result.AppendLine($"services.AddTransient<I{symbol.Name} ,{symbol.Name}>();");
            }
            return result;
        }

        private bool RegistrationsIsNull(StringBuilder? registrations) => (registrations == null) ? true : false;
        private static string GetFullName(ISymbol symbol)
        {
            var ns = symbol.ContainingNamespace;
            var nss = new List<string>();
            while (ns != null)
            {
                if (string.IsNullOrWhiteSpace(ns.Name))
                    break;
                nss.Add(ns.Name);
                ns = ns.ContainingNamespace;
            }
            nss.Reverse();
            if (nss.Any())
                return $"{string.Join(".", nss)}.{symbol.Name}";
            return symbol.Name;
        }
    }
}
