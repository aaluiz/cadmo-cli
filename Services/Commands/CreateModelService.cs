using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;

[AddService]
public class CreateModelService : AbstractService, ICreateModelService
{


	ICodeGenerator _codeGenerator;
	public CreateModelService(ICodeGenerator codeGenerator)
	{
		_codeGenerator = codeGenerator;
	}
	public int Execute(string[] args)
	{
        if (!ValidateArgs(args)) return -1;

		if (args[2].Contains("-")) return -1;

		if (IsDefaultPath(args[0]))
		{
            _codeGenerator.FileBuilder!.WriteFile(BasicModel(args[2]), $"{CurrentDirectory}/Entities/Models");
            _codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}UpdateViewModel"), $"{CurrentDirectory}/Entities/ViewModels");
            _codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}NewViewModel"), $"{CurrentDirectory}/Entities/ViewModels");
            _codeGenerator.FileBuilder!.WriteFile(BasicViewModel($"{args[2]}ViewModel"), $"{CurrentDirectory}/Entities/ViewModels");
		}
		System.Console.WriteLine("Models and BasicViewModels created!");
		System.Console.WriteLine($"{CurrentDirectory}/Entities/Models/{args[2]}.cs");
		System.Console.WriteLine($"{CurrentDirectory}/Entities/ViewModels/{args[2]}UpdateViewModel.cs");
		System.Console.WriteLine($"{CurrentDirectory}/Entities/ViewModels/{args[2]}NewViewModel.cs");
		System.Console.WriteLine($"{CurrentDirectory}/Entities/ViewModels/{args[2]}ViewModel.cs");
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
			"Entities.Interface"
		}.ToImmutableList();

		var inharitance = new string[] {
			"IEntity"
		}.ToImmutableList();

		var result = _codeGenerator.ClassGenerator.CreateClass( imports ,name, "Entities.Models", property, inharitance);
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
			"Entities.Interface"
		}.ToImmutableList();

		var inharitance = new string[] {
			"IEntityViewModel"
		}.ToImmutableList();

		var result = _codeGenerator.ClassGenerator.CreateClass( imports ,name, "Entities.ViewModels", property, inharitance);
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
		}.ToImmutableList();
	}

	protected override bool ValidateArgs(string[] args)
	{
		if (!IsValidArgs(args)) return false;
		return IsTheReservedWord("model", args);
	}
}