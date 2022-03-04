using System.Collections.Immutable;
using Models;

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

			var imports = new string[] {
			"System.ComponentModel",
			"System.ComponentModel.DataAnnotations"
			}.ToImmutableList();

			var result = _codeGenerator
				.ClassGenerator
				.CreateClass(imports, $"{modelName}ViewModel", "Entities.ViewModels", properties.ToImmutableList());
			return result;

		}

		private FileCode GenerateViewModelUpdateOrNew(string modelName, bool update)
		{
			ModelJson modelJson = GetContentJsonFile(modelName);

			var propertiesFromJson = modelJson.Model!.Fields!.Where(x => x.ViewModelVisibility).Select(field =>
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

			var imports = new string[] {
			"System.ComponentModel",
			"System.ComponentModel.DataAnnotations",
			"System.ComponentModel.DataAnnotations.Schema",
			}.ToImmutableList();

			var result = _codeGenerator
				.ClassGenerator
				.CreateClass(imports, $"{modelName}{((update) ? "Update" : "New")}ViewModel", "Entities.ViewModels", properties.ToImmutableList());
			return result;

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

			var result = _codeGenerator
				.ClassGenerator
				.CreateClass(imports, modelName, "Entities.Models", properties.ToImmutableList(), inharitance);
			return result;

		}
    }
}