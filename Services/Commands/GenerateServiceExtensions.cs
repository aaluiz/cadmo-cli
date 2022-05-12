using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;
using System.Text;

namespace Services.Commands
{
    [AddService]
    public class GenerateServiceExtensions : AbstractService, IGenerateServiceExtensions
    {
        private readonly ICodeGenerator _codeGenerator;
        private readonly IMethodDefinition _methodDefiniton;
        private readonly IDirectoryHandler _directoryHandler;

        public GenerateServiceExtensions(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition, IDirectoryHandler directoryHandler)
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
            System.Console.WriteLine("GENERATED Service Dependency Injection Extension File.");
            System.Console.WriteLine($"{CurrentDirectory}/Api/Extensions/ServiceExtensions.cs");
        }
        private FileCode GetFileCode()
        {
            var imports = new string[]{
                "Contracts.Service",
                "Contracts",
                "Entities.Models",
                "Microsoft.Extensions.DependencyInjection",
                "Services",
            }.ToImmutableList();

            var methods = ConfigureServiceMethod();
            _codeGenerator.ClassGenerator.IsInterface = false;
            _codeGenerator.ClassGenerator.IsStatic = true;
            var result = _codeGenerator.ClassGenerator
                .CreateClass(imports, "ServicesExtensions", "Api.Extensions", methods);

            return result;
        }

        private ImmutableList<IMethodDefinition> ConfigureServiceMethod()
        {
            var parameters = new Parameter[]{
                new Parameter{
                    Type = "this IServiceCollection",
                    Name = "Services"
                },
            }.ToImmutableList();

            var method = _methodDefiniton.Builder
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

            result.AppendLine("Services.AddTransient<ILoggerManager, LoggerManager>();");

            _directoryHandler.GetServiceNames(CurrentDirectory).ForEach((model) =>
            {
                result.AppendLine($"Services.AddTransient<I{model}, {model}>();");
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