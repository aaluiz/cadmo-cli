using Services.Abstract;
using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;
using System.Text;

namespace Services.Commands
{

    [AddService]
	public class GenerateServicesDI : AbstractService, IGenerateServicesDI
	{

        
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methoDefinition;
		private readonly IDirectoryHandler _diretoryHandler;
		public GenerateServicesDI(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition, IDirectoryHandler directoryHandler)
		{
			_codeGenerator = codeGenerator;
			_methoDefinition = methodDefinition;
			_diretoryHandler = directoryHandler;
		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			_diretoryHandler.DeleteClass1File(CurrentDirectory, "Services");
			WriteFile();
			return 1;
		}

        private void WriteFile(){
			_codeGenerator
                .FileBuilder
                    .WriteFile(GetFileCode(), 
                    $"{CurrentDirectory}/Api/Extensions/"
                    );
					System.Console.WriteLine("GENERATED Service Dependency Injection Extension File.");
					System.Console.WriteLine($"{CurrentDirectory}/Api/Extensions/ServicesExtensions.cs");
		}
		private FileCode GetFileCode()
		{
			var imports = new string[]{
				"Contracts.Service",
				"Contracts",
				"Microsoft.Extensions.DependencyInjection",
				"Services",
			}.ToImmutableList();

			var methods = ConfigureRepositoryMethod();
			_codeGenerator.ClassGenerator.IsInterface = false;
			_codeGenerator.ClassGenerator.IsStatic = true;
			var result = _codeGenerator.ClassGenerator
				.CreateClass(imports, "ServicesExtensions", "Api.Extensions", methods);

			return result;
		}

		private ImmutableList<IMethodDefinition> ConfigureRepositoryMethod()
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = "this IServiceCollection",
					Name = "services"
				},
			}.ToImmutableList();

			var method = _methoDefinition.Builder
			.Name("ConfigureServices")
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

			_diretoryHandler.GetServiceNames(CurrentDirectory).ForEach((service) =>
			{
				result.AppendLine($"services.AddTransient<I{service}, {service}>();");
			});

			return result.ToString();
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("service-di", args);
		}
	}
}