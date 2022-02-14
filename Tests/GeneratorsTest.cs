using System.Collections.Immutable;
using Contracts.Interfaces;
using NUnit.Framework;
using Services;
using Services.Coder;
using Models;
using System.Collections.Generic;
using Moq;

namespace Tests
{
	[TestFixture]
	public class GeneratorsTest
	{
		IClassDefinition? _classDefiner;
		IMethodDefinition? _methodDefinition;

		ICreateClassGenerator? _createClassGenerator;
		ICreateInterfaceGenerator? _interfaceGenerator;

		[SetUp]
		public void Setup()
		{
			_classDefiner = new ClassDefinition(new BuilderClassDefinition());
			_methodDefinition = new MethodDefinition(new BuilderMethodDefinition());

			var fileBuilderMock = new Mock<IFileBuilder>();
			fileBuilderMock.Setup(x => x.WriteFile(new FileCode(), "/user/path")).Returns(true);
			fileBuilderMock.Setup(x => x.WriteFiles(new List<FileCode>().ToImmutableList(), "/user/path")).Returns(true);

			_createClassGenerator = new CreateClassGenerator(_classDefiner, _methodDefinition, fileBuilderMock.Object);
			_interfaceGenerator = new CreateInterfaceGenerator(_classDefiner, _methodDefinition, fileBuilderMock.Object);
		}

		[Test]
		public void CreateClassGenerator_CreateClass_ReturnCode()
		{
			var result = _createClassGenerator!.CreateClass("Writer", "RoselynCompileSample");
			var validation = CSharpCompiler.ValidateSourceCode(result.Code!);

			Assert.AreEqual(result.FileName, "Writer.cs");
			Assert.IsTrue(validation);
		}

		[Test]
		public void CreateClassGenerator_CreateClass_WithMethods_ReturnCode()
		{
			var methods = new List<IMethodDefinition>();

			methods.Add(_methodDefinition!.Builder
			.Name("NovoMetodos")
			.Create());

			var result = _createClassGenerator!.CreateClass(new string[] { "System" }.ToImmutableList(), "Writer", "RoselynCompileSample", methods.ToImmutableList());
			var validation = CSharpCompiler.ValidateSourceCode(result.Code!);

			Assert.AreEqual(result.FileName, "Writer.cs");
			Assert.IsTrue(validation);
		}

		[Test]
		public void CreateClassGenerator_Properties_Complete_ReturnCode()
		{
			var properties = new List<Property>();
			properties.Add(new Property
			{
				Name = "Id",
				TypeProperty = "int"
			});
			var result = _createClassGenerator!.CreateClass(new string[] { "System" }.ToImmutableList(), "Writer", "RoselynCompileSample", properties.ToImmutableList());
			var validation = CSharpCompiler.ValidateSourceCode(result.Code!);

			Assert.AreEqual(result.FileName, "Writer.cs");
			Assert.IsTrue(validation);
		}

		[Test]
		public void CreateClassGenerator_Complete_Properties_ReturnCode()
		{
			var methods = new List<IMethodDefinition>();

			methods.Add(_methodDefinition!.Builder
				.Name("NovoMetodos")
				.Create());

			var properties = new List<Property>();
			properties.Add(new Property
			{
				Name = "Id",
				TypeProperty = "int"
			});
			var result = _createClassGenerator!.CreateClass(new string[] { "System" }.ToImmutableList(), "Writer", "RoselynCompileSample", methods.ToImmutableList(),properties.ToImmutableList());
			var validation = CSharpCompiler.ValidateSourceCode(result.Code!);

			Assert.AreEqual(result.FileName, "Writer.cs");
			Assert.IsTrue(validation);
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