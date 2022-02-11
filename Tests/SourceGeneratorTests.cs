using NUnit.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using SourceGenerator;

namespace Tests
{

	[TestFixture]
	public class SourceGeneratorTests
	{


		[Test]
		public void GeneratedCodeWithoutServicesWork()
		{
			var source = @"
using Microsoft.Extensions.DependencyInjection;
namespace SourceGeneratorWeb.Code
{
    class C
    {
        void M(IServiceCollection services)
        {
            services.AddServicesToDI();
        }
    }
}";
			var (attributeCode, extensionCode) = GetGeneratedOutput(source);
			Assert.IsNotNull(attributeCode);
			Assert.IsNotNull(extensionCode);

			const string expectedAttributeCode = @"// <auto-generated />
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class AddServiceAttribute : System.Attribute
{
}
";
			const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;
using Services.Coder;
using Services.Startup;
using Contracts.Interfaces;

namespace Services
{
    public static class GeneratedServicesExtension
    {
        public static void AddServicesToDI(this IServiceCollection services)
        {
        }
    }
}
";

			Console.WriteLine("Gerado - extensionCode:");
			Console.WriteLine(extensionCode);

			Console.WriteLine("Esperado - expectedExtensionCode:");
			Console.WriteLine(expectedExtensionCode);

			Console.WriteLine("Gerado - attibuteCode:");
			Console.WriteLine(attributeCode);

			Console.WriteLine("Esperado - expectedAttributeCode:");
			Console.WriteLine(expectedAttributeCode);

			Assert.AreEqual(attributeCode, expectedAttributeCode);
			Assert.AreEqual(extensionCode, expectedExtensionCode);

		}
		[Test]
		public void GeneratedCodeWithOneService()
		{
			var source = @"
using Microsoft.Extensions.DependencyInjection;
namespace SourceGeneratorWeb.Code
{
    class C
    {
        void M(IServiceCollection services)
        {
             services.AddServicesToDI();
        }
    }
    [AddService]
    class MyService
    {
    }
}";
			var (_, extensionCode) = GetGeneratedOutput(source);

			Assert.IsNotNull(extensionCode);

			const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;
using Services.Coder;
using Services.Startup;
using Contracts.Interfaces;

namespace Services
{
    public static class GeneratedServicesExtension
    {
        public static void AddServicesToDI(this IServiceCollection services)
        {
            services.AddTransient<IMyService ,MyService>();
        }
    }
}
";
			Assert.AreEqual(extensionCode, expectedExtensionCode);
		}
		[Test]
		public void GeneratedCodeWithOneServiceDiferentNameSpace()
		{
			var source = @"
using Microsoft.Extensions.DependencyInjection;
namespace Service.Code
{
    class C
    {
        void M(IServiceCollection services)
        {
             services.AddServicesToDI();
        }
    }
    [AddService]
    class MyService
    {
    }
}";
			var (_, extensionCode) = GetGeneratedOutput(source);

			Assert.IsNotNull(extensionCode);

			const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;
using Services.Coder;
using Services.Startup;
using Contracts.Interfaces;

namespace Services
{
    public static class GeneratedServicesExtension
    {
        public static void AddServicesToDI(this IServiceCollection services)
        {
            services.AddTransient<IMyService ,MyService>();
        }
    }
}
";
			Assert.AreEqual(extensionCode, expectedExtensionCode);
		}
		[Test]
		public void GeneratedCodeWithTwoServices()
		{
			var source = @"
using Microsoft.Extensions.DependencyInjection;
namespace SourceGeneratorWeb.Code
{
    class C
    {
        void M(IServiceCollection services)
        {
             services.AddServicesToDI();
        }
    }
    [AddService]
    class MyService1
    {
    }
    [AddService]
    class MyService2
    {
    }
}";
			var (_, extensionCode) = GetGeneratedOutput(source);
			Assert.IsNotNull(extensionCode);

			const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;
using Services.Coder;
using Services.Startup;
using Contracts.Interfaces;

namespace Services
{
    public static class GeneratedServicesExtension
    {
        public static void AddServicesToDI(this IServiceCollection services)
        {
            services.AddTransient<IMyService1 ,MyService1>();
            services.AddTransient<IMyService2 ,MyService2>();
        }
    }
}
";

			Console.WriteLine("Gerado - extensionCode:");
			Console.WriteLine(extensionCode);

			Console.WriteLine("Esperado - expectedExtensionCode:");
			Console.WriteLine(expectedExtensionCode);
			Assert.AreEqual(extensionCode, expectedExtensionCode);
		}

		private (string, string) GetGeneratedOutput(string source)
		{
			var outputCompilation = CreateCompilation(source);
			var trees = outputCompilation.SyntaxTrees.Reverse().Take(2).Reverse().ToList();
			foreach (var tree in trees)
			{
				Console.WriteLine(Path.GetFileName(tree.FilePath) + ":");
				Console.WriteLine(tree.ToString());
			}
			return (trees.First().ToString(), trees[1].ToString());
		}

		private static Compilation CreateCompilation(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);

			var references = new List<MetadataReference>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
					references.Add(MetadataReference.CreateFromFile(assembly.Location));
			references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.Extensions.DependencyInjection.IServiceCollection).Assembly.Location));

			var compilation = CSharpCompilation.Create("foo",
																								 new SyntaxTree[] { syntaxTree },
																								 references,
																								 new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			var generator = new Generator();

			var driver = CSharpGeneratorDriver.Create(generator);
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

			var compileDiagnostics = outputCompilation.GetDiagnostics();


			bool compileErrors = compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
			if (compileErrors) Console.WriteLine("Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

			bool generateErros = generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
			if (generateErros) Console.WriteLine("Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

			return outputCompilation;
		}
	}
}
