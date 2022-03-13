using Services.Abstract;
using Contracts.Interfaces;
using System.Collections.Immutable;
using Models;
namespace Services.Commands
{
	[AddService]
	public class CreateRepositoryService : AbstractService, ICreateRepositoryService
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;

		public CreateRepositoryService(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			WriteFile(args[2]);
			return 1;
		}

		private void WriteFile(string model)
		{
			_codeGenerator
				.FileBuilder
					.WriteFile(GetFileCode(model),
					$"{CurrentDirectory}/Repositories/");
			System.Console.WriteLine($"GENERATED ../Repositories/{model}Repository.cs");

			_codeGenerator
				.FileBuilder
					.WriteFile(GetInterfaceFileCode(model),
					$"{CurrentDirectory}/Contracts/Repository/");
			System.Console.WriteLine($"GENERATED ..Contracts/Repository/I{model}Repository.cs");

		}

		private FileCode GetFileCode(string modelName)
		{
			var imports = new string[]{
				"Contracts",
				"Contracts.Repository",
				"Entities.Data",
				"Entities.Models",
				"Entities.Models.Enums",
				"Microsoft.Extensions.Configuration",
				"Repositories.Abstract",
				"System.Linq"
			}.ToImmutableList();

			var inharitage = new string[]{
				$"RepositoryAbstract<{modelName}>, I{modelName}Repository<{modelName}>",
			}.ToImmutableList();

			var methods = ConstructoWithBaseObjectAction(modelName);
			_codeGenerator.ClassGenerator.IsInterface = false;
		
			var result = _codeGenerator.ClassGenerator
				.CreateClass(imports, $"{modelName}Repository", "Repositories", methods, inharitage);

			return result;
		}

		private ImmutableList<IMethodDefinition> ConstructoWithBaseObjectAction(string modelName)
		{
			var baseObject = new string[]{
				"context",
				"Logger",
				"configuration"
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = "ApiDbContext",
					Name = "context"
				},
				new Parameter{
					Type = "ILoggerManager",
					Name = "Logger"
				},
				new Parameter{
					Type = "IConfiguration",
					Name = "configuration"
				},
			}.ToImmutableList();

			var constructor = _methodDefinition.Builder
			.Name($"{modelName}Repository")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.LogiContent($"ColllectionFromDataBase = \"{modelName}s\";")
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return new IMethodDefinition[] { constructor }.ToImmutableList();
		}

		private FileCode GetInterfaceFileCode(string modelName)
		{
			var imports = new string[]{
				"Contracts.Repository.Abstract",
			}.ToImmutableList();

			var inharitage = new string[]{
				$"IRepository<Model>",
			}.ToImmutableList();

			_codeGenerator.ClassGenerator.IsInterface = true;
			var result = _codeGenerator.ClassGenerator
				.CreateClass(imports, $"I{modelName}Repository<Model>", "Contracts.Repository", inharitage);

			return result;
		}


		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			if (!ModelExist(args)) return false;
			return IsTheReservedWord("repository", args);
		}

	}
}