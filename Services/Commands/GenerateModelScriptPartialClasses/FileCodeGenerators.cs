using Models;
using System.Collections.Immutable;

namespace Services.Commands
{
    public partial class GenerateModelByScript
    {
        private FileCode GenerateViewModel(string modelName)
        {
            ModelJson modelJson = GetContentJsonFile(modelName);

            var propertiesFromJson = modelJson.Model!.Fields!.Where(x => x.ViewModelVisibility).Where(y => y.ForeignKey == null).Select(field =>
            {
                return ProcessFieldViewModel(field);
            }).ToList();

            var properties = new Property[]{
            new Property{
                Name = "Id",
                Visibility = Visibility.Public,
                hasGeterAndSeter = true,
                TypeProperty = "int"
            }
            }.ToList();

            properties.AddRange(propertiesFromJson);
            var heritage = new string[] { "IEntityViewModel" }.ToImmutableList();

            var extraDependencies = new List<string>();
            var imports = new List<string>();
            imports = GenExtraDependencies(modelJson, ref extraDependencies);
            imports.Add("Entities.Interface");

            var result = _codeGenerator
                .ClassGenerator
                .CreateClass(imports.ToImmutableList(), $"{modelName}ViewModel", "Entities.ViewModels", properties.ToImmutableList(), heritage);
            return result;

        }

        private FileCode GenerateViewModelUpdateOrNew(string modelName, bool update)
        {
            ModelJson modelJson = GetContentJsonFile(modelName);

            var propertiesFromJson = modelJson.Model!.Fields!.Where(x => x.ViewModelVisibility).Select(field =>
            {
                return ProcessField(field);
            }).ToList();

            var properties = Array.Empty<Property>().ToList();

            if (update)
            {
                properties.Add(
                new Property
                {
                    Name = "Id",
                    Visibility = Visibility.Public,
                    hasGeterAndSeter = true,
                    TypeProperty = "int"
                });
            }

            var heritage = new string[] { "IEntityViewModel" }.ToImmutableList();

            properties.AddRange(propertiesFromJson);

            var extraDependencies = new List<string>();
            var imports = new List<string>();
            imports = GenExtraDependencies(modelJson, ref extraDependencies);
            imports.Add("Entities.Interface");

            var result = _codeGenerator
                .ClassGenerator
                .CreateClass(imports.ToImmutableList(), $"{modelName}{((update) ? "Update" : "New")}ViewModel", "Entities.ViewModels", properties.ToImmutableList(), heritage);
            return result;

        }

        private static List<string> GenExtraDependencies(ModelJson modelJson, ref List<string> extraDependencies)
        {
            List<string> imports;
            if (modelJson.Model!.Dependencies != null)
            {
                extraDependencies = modelJson.Model.Dependencies.Select(x => x.Package).ToList()!;
                imports = new string[] {
                    "System.ComponentModel",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema",
                    "Entities.Models",
                }.ToList();
                imports.AddRange(extraDependencies);


            }
            else
            {
                imports = new string[] {
                    "System.ComponentModel",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema",
                    "Entities.Models",
                }.ToList();
            }

            return imports;
        }

        private FileCode GeneraBasicModelCode(string modelName)
        {
            ModelJson modelJson = GetContentJsonFile(modelName);

            var propertiesFromJson = modelJson.Model!.Fields!.Select(field =>
            {
                return ProcessField(field);
            }).ToList();

            var properties = new Property[]{
            new Property{
                Name = "Id",
                Visibility = Visibility.Public,
                hasGeterAndSeter = true,
                TypeProperty = "int"
            }
            }.ToList();

            properties.AddRange(propertiesFromJson);

            var extraDependencies = new List<string>();
            var imports = new List<string>();
            imports = GenExtraDependencies(modelJson, ref extraDependencies);
            imports.Add("Entities.Interface");

            var inharitance = new string[] {
            "IEntity"
            }.ToImmutableList();

            var result = _codeGenerator
                .ClassGenerator
                .CreateClass(imports.ToImmutableList(), modelName, "Entities.Models", properties.ToImmutableList(), inharitance);
            return result;

        }
    }
}