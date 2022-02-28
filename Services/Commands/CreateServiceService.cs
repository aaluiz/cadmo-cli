using System.Collections.Immutable;
using Contracts.Interfaces;
using Services.Abstract;

namespace Services.Commands
{
	[AddService]
	public partial class CreateServiceCrudService : AbstractService, ICreateServiceCrudService
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;
		private readonly IDirectoryHandler _directoroyHandler;

		public CreateServiceCrudService(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition, IDirectoryHandler directoryHandler)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
			_directoroyHandler = directoryHandler;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			var (models, _) = ModelsExits(args);
			GenerateInteface(models.First());
			WriteService(models);
			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;

			var (models, modelsExits) = ModelsExits(args);

			if (!modelsExits) return false;


			return IsTheReservedWord("service-crud", args);
		}

		private (ImmutableList<string>, bool) ModelsExits(string[] args)
		{
			var models = (args[3].Contains(',')) ? args[3].Split(",").ToImmutableList() : new List<string> { args[3] }.ToImmutableList();

			var inDirectory = _directoroyHandler.GetModelNames(CurrentDirectory);

			bool result = true;

			models.ForEach((model) =>
			{
				if (!inDirectory.Contains(model)) result = false;
			});

			return (models, result);
		}
	}
}