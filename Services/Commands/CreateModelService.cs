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

		if (IsDefaultPath(args[0]))
		{
            _codeGenerator.FileBuilder!.WriteFile(BasicModel(args[0]), $"{CurrentDirectory}/Entities/Models");
		}
		return 1;
	}

	private FileCode BasicModel(string name)
	{
		var result = _codeGenerator.ClassGenerator.CreateClass(name, "Entites.Models");
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