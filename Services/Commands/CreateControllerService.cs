using Contracts.Interfaces;
using System.Collections.Immutable;
using Services.Abstract;

namespace Services.Commands
{
    [AddService]
	public partial class CreateControllerService : AbstractService, ICreateControllerService
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;
		private readonly IDirectoryHandler _diretoryHandler;

		public CreateControllerService(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition, IDirectoryHandler directoryHandler)
        {
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
			_diretoryHandler = directoryHandler;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			var (models, _) = ModelsExits(args);
			if (args.Length == 5){
				if (args[4] == "--secure") {
					WriteController(models.First(), true);
				} else {
					return -1;
				}
			} else {
				WriteController(models.First(), false);
			}

			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;

			if (args[2] != "--model") return false;

			var (models, modelsExits) = ModelsExits(args);
			if (!modelsExits) return false;

			return IsTheReservedWord("controller-crud", args);
		}
        
		private (ImmutableList<string>, bool) ModelsExits(string[] args)
		{
			var models = (args[3].Contains(',')) ? args[3].Split(",").ToImmutableList() : new List<string> { args[3] }.ToImmutableList();

			var inDirectory = _diretoryHandler.GetModelNames(CurrentDirectory);

			bool result = true;

			models.ForEach((model) =>
			{
				if (!inDirectory.Contains(model)) result = false;
			});

			return (models, result);
		}
	}
}