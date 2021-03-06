using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;
using Spectre.Console;

[AddService]
public class CreateModelService : AbstractService, ICreateModelService
{
	ICodeGenerator _codeGenerator;
	private readonly IShellCommandExecutor _shellCommandExecutor;

	public CreateModelService(ICodeGenerator codeGenerator, IShellCommandExecutor shellCommandExecutor)
	{
		_codeGenerator = codeGenerator;
		_shellCommandExecutor = shellCommandExecutor;
	}
	public int Execute(string[] args)
	{
		if (!ValidateArgs(args)) return -1;

		if (args[2].Contains("-")) return -1;

		if (IsDefaultPath(args[0]))
		{
			_codeGenerator.FileBuilder!.WriteFile(BasicModel(args[2]), $"{CurrentDirectory}/Entities/Models");

			_codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}UpdateViewModel"), $"{CurrentDirectory}/Entities/ViewModels/{args[2]}/");
			_codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}NewViewModel"), $"{CurrentDirectory}/Entities/ViewModels/{args[2]}/");
			_codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}ViewModel"), $"{CurrentDirectory}/Entities/ViewModels/{args[2]}/");
		}
		AnsiConsole.Markup("[green] CREATED [/] Models and BasicViewModels.");
		AnsiConsole.Markup($"{CurrentDirectory}/Entities/Models[blue]{args[2]}.cs[/]");
		AnsiConsole.Markup($"{CurrentDirectory}/Entities/ViewModels/{args[2]}/[blue]{args[2]}UpdateViewModel.cs[/]");
		AnsiConsole.Markup($"{CurrentDirectory}/Entities/ViewModels/{args[2]}/[blue]{args[2]}NewViewModel.cs[/]");
		AnsiConsole.Markup($"{CurrentDirectory}/Entities/ViewModels/{args[2]}/[blue]{args[2]}ViewModel.cs[/]");
		return 1;
	}

	private FileCode BasicModel(string name)
	{
		var property = new Property[]{
			new Property{
				Name = "Id",
				Visibility = Visibility.Public,
				hasGeterAndSeter = true,
				TypeProperty = "int"
			}
		}.ToImmutableList();
		var imports = new string[] {
			"System.Collections.Generic",
			"System.ComponentModel",
			"System.ComponentModel.DataAnnotations",
			"System.ComponentModel.DataAnnotations.Schema",
			"Entities.Interface"
		}.ToImmutableList();

		var inharitance = new string[] {
			"IEntity"
		}.ToImmutableList();

		var result = _codeGenerator.ClassGenerator.CreateClass(imports, name, "Entities.Models", property, inharitance);
		return result;
	}

	private FileCode BasicViewModel(string name)
	{
		var property = new Property[]{
			new Property{
				Name = "Id",
				Visibility = Visibility.Public,
				hasGeterAndSeter = true,
				TypeProperty = "int"
			}
		}.ToImmutableList();
		var imports = new string[] {
			"System.ComponentModel.DataAnnotations",
			"System.ComponentModel.DataAnnotations.Schema",
			"Entities.Models",
			"Entities.Interface",
		}.ToImmutableList();

		var inharitance = new string[] {
			"IEntityViewModel"
		}.ToImmutableList();

		var result = _codeGenerator.ClassGenerator.CreateClass(imports, name, "Entities.ViewModels", property, inharitance);
		return result;
	}


	private ImmutableList<Property> GetPropertiesBasicModel()
	{
		return new Property[] {
			new Property
			{
			  Name = "Id",
			  TypeProperty = "int",
			  Visibility = Visibility.Public
			}
		}.ToImmutableList();

	}

	private static ImmutableList<string> GetImportsBasicModel()
	{
		return new string[] {
		"Entities.Interface",
		"Entities.Models.Enums",
		"System.Collections.Generic",
		"System.ComponentModel",
		"System.ComponentModel.DataAnnotations",
		"System.ComponentModel.DataAnnotations.Schema",
		}.ToImmutableList();
	}

	protected override bool ValidateArgs(string[] args)
	{
		if (!IsValidArgs(args)) return false;
		return IsTheReservedWord("model", args);
	}
}
