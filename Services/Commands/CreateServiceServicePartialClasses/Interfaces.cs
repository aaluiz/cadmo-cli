
using System.Collections.Immutable;
using Contracts.Interfaces;
using Services.Abstract;

namespace Services.Commands
{
	public partial class CreateServiceCrudService
	{
		private void GenerateInteface(string mainModel)
		{
			var imports = new string[] { "Contracts.Service.Abstract", "Entities.ViewModels" }.ToImmutableList();

			var heritage = new string[] {
				$"IInsert<{mainModel}NewViewModel>",
				$"IUpdate<{mainModel}UpdateViewModel>",
				"IDelete",
				$"ISelectAllViews<{mainModel}ViewModel>",
				$"ISelectOneView<{mainModel}ViewModel>",
				"IControlError"
			 }.ToImmutableList();

			var fileCode = _codeGenerator
				.InterfaceGenerator
					.CreateInterface(imports, $"I{mainModel}Service", "Contracts.Service", heritage);

			_codeGenerator
				.FileBuilder
					.WriteFile(fileCode, $"{CurrentDirectory}/Contracts/Service/");
			System.Console.WriteLine($"GENERATED ../Contratcts/Service/I{mainModel}Service.cs");
		}
	}
}