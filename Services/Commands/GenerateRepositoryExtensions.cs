using Contracts.Interfaces;
using Services.Abstract;
using Models;
using System.Collections.Immutable;
using System.Text;

namespace Services.Commands
{
	[AddService]
	public class GenerateRepositoryExtensions : AbstractService, IGenerateRepositoryExtensions
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefiniton;
		private readonly IDirectoryHandler _directoryHandler;

		public GenerateRepositoryExtensions(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition, IDirectoryHandler directoryHandler)
		{
			_codeGenerator = codeGenerator;
			_methodDefiniton = methodDefinition;
			_directoryHandler = directoryHandler;
		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			WriteFile();
			return 1;
		}

		private void WriteFile()
		{
			_codeGenerator
				.FileBuilder
					.WriteFile(GetFileCode(),
					$"{CurrentDirectory}/Api/Extensions/"
					);
			System.Console.WriteLine("GENERATED Repository Dependency Injection Extension File.");
			System.Console.WriteLine($"{CurrentDirectory}/Api/Extensions/RepositoryExtensions.cs");
		}
		private FileCode GetFileCode()
		{
			var imports = new string[]{
				"Contracts.Repository",
				"Entities.Models",
				"Microsoft.Extensions.DependencyInjection",
				"Repositories",
			}.ToImmutableList();

			var methods = ConfigureRepositoryMethod();
			_codeGenerator.ClassGenerator.IsInterface = false;
			_codeGenerator.ClassGenerator.IsStatic = true;
			var result = _codeGenerator.ClassGenerator
				.CreateClass(imports, "RepositoryExtensions", "Api.Extensions", methods);

			return result;
		}

		private ImmutableList<IMethodDefinition> ConfigureRepositoryMethod()
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = "this IServiceCollection",
					Name = "Services"
				},
			}.ToImmutableList();

			var method = _methodDefiniton.Builder
			.Name("ConfigureRepository")
			.Parameters(parameters)
			.LogiContent(LogiContent())
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "static void",
				Visibility = Visibility.Public
			})
			.Create();
			return new IMethodDefinition[] { method }.ToImmutableList();
		}

		private string LogiContent()
		{
			StringBuilder result = new StringBuilder();

			if (AlreadyExitsSomeRepository(CurrentDirectory))
			{
				_directoryHandler.GetRespoistoryNames(CurrentDirectory).ForEach((repositoryName) =>
				{
					string model = ExtractModelName(repositoryName);
					result.AppendLine($"Services.AddTransient<I{repositoryName}<{model}>, {repositoryName}>();");
				});
			}
			else
			{
				_directoryHandler.GetModelNames(CurrentDirectory).ForEach((model) =>
				{
					result.AppendLine($"Services.AddTransient<I{model}Repository<{model}>, {model}Repository>();");
				});
			}


			return result.ToString();
		}

		private string ExtractModelName(string repositoryName){
			return repositoryName.Replace("Repository", "");
		}

		private bool AlreadyExitsSomeRepository(string currentDirectory)
		{
			int repositoryCount = _directoryHandler.GetRespoistoryNames(currentDirectory).Count();
			return (repositoryCount > 0);
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("repository-di", args);
		}
	}
}