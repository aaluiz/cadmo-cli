using System.Collections.Immutable;
using Contracts.Interfaces;
using NUnit.Framework;
using Services;
using Services.Coder;
using Models;
using System.Collections.Generic;

namespace Tests
{
	[TestFixture]
	public class GeneratorsTest
	{
		IClassDefinition? _classDefiner;
		IMethodDefinition? _methodDefinition;
		[SetUp]
		public void Setup()
		{
			_classDefiner = new ClassDefinition(new BuilderClassDefinition());
			_methodDefinition = new MethodDefinition(new BuilderMethodDefinition());
		}


		[Test]
		public void ClassBusinnesLayer_ReturnStringClassCode()
		{
			var Parameters = new List<Parameter>();
			Parameters.Add(
				new Parameter
				{
					Name = "message",
					Type = "string"
				}
			);

			var Method = _methodDefinition!.Builder
			.Name("Write")
			.Parameters(Parameters.ToImmutableList())
			.LogiContent(@" Console.WriteLine($""you said '{message}!'"");")
			.Create();

			var Methods = new List<IMethodDefinition>();
			Methods.Add(Method);

			//arrange
			var imports = new string[] { "System", "Contracts" }.ToImmutableList();
			var businnessLayerService = _classDefiner!.Builder
				.Imports(imports)
				.Namespace("RoslynCompileSample")
				.Name("Writer")
                .Methods(Methods.ToImmutableList())
				.Create();

			//act
			string result = businnessLayerService.ClassCode;

			var validation = CSharpCompiler.ValidateSourceCode(result);


			Assert.IsTrue(validation);
			//assert
			Assert.IsTrue(result.Contains("class"));
			Assert.IsTrue(result.Contains("}"));
		}

	}
}