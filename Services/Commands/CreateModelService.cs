using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;

[AddService]
public class CreateModelService : AbstractGeneratorService, ICreateModelService
{
    public CreateModelService(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder) : base(classDefinition, methodDefinition, fileBuilder)
    {
    }

    public int Execute(string[] args)
    {
        if (IsDefaultPath(args[0]))
        {
            _fileBuilder!.WriteFile(BasicModel(args[0]), $"{CurrentDirectory}/Entities/Models");
        }
        return 1;
    }

    private FileCode BasicModel(string name)
    {
        var builder = _classDefinition!.Builder
            .Namespace("Entities.Models")
            .Imports(GetImportsBasicModel())
            .Name(name)
            .Properties(GetPropertiesBasicModel())
            .Create();
        var result = new FileCode { Code = builder.ClassCode, FileName = $"{name}.cs" };
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

}