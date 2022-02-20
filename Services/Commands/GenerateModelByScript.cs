using Services.Abstract;
using Contracts.Interfaces;
using Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Immutable;
using System.Text;
using Newtonsoft.Json;

namespace Services.Commands
{

	[AddService]
	public class GenerateModelByScript : AbstractService, IGenerateModelByScript
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;

		public GenerateModelByScript(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			switch (args[2])
			{
				case "--with-script": return GenerateModel(args[3]);
				case "--with-all-scripts":
					switch (args[3])
					{
						case "--safety": return SafetyGenerate();
						case "--force": return ForceGenerate();
						default:
							return -1;
					}
				default:
					return -1;
			}
		}

		private int ForceGenerate()
		{

			return 1;
		}

		private int SafetyGenerate()
		{

			return 1;
		}

		private int GenerateModel(string scriptModelName)
		{
			SaveFileOnDisk(GeneraBasicModelCode(scriptModelName), "Models");
			SaveFileOnDisk(GenerateViewModel(scriptModelName), "ViewModels");
			SaveFileOnDisk(GenerateViewModelUpdateOrNew(scriptModelName, true), "ViewModels");
			SaveFileOnDisk(GenerateViewModelUpdateOrNew(scriptModelName, false), "ViewModels");
			return 1;
		}

		private void SaveFileOnDisk(FileCode fileCode, string path)
		{
			_codeGenerator
				.FileBuilder!
				.WriteFile(fileCode, $"{CurrentDirectory}/Entities/{path}");
			System.Console.WriteLine($"GENERATED {CurrentDirectory}/Entities/{path}/{fileCode.FileName}");
		}
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

			var inharitance = new string[] {
			"IEntity"
			}.ToImmutableList();

			var result = _codeGenerator
				.ClassGenerator
				.CreateClass(imports, modelName, "Entities.ViewModels", properties.ToImmutableList(), inharitance);
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
			"System.ComponentModel.DataAnnotations"
			}.ToImmutableList();

			var inharitance = new string[] {
			"IEntity"
			}.ToImmutableList();

			var result = _codeGenerator
				.ClassGenerator
				.CreateClass(imports, $"{((update)? "Update": "New")}" + modelName, "Entities.ViewModels", properties.ToImmutableList(), inharitance);
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

		private ModelJson GetContentJsonFile(string ModelName)
		{
			string pathModel = $"{CurrentDirectory}/Entities/JsonModelsDefinition/{ModelName}.json";
			string contentFile = GetContentFile(pathModel);
			var result = JsonConvert.DeserializeObject<ModelJson>(contentFile);
			return result!;
		}

		private string GetContentFile(string path)
		{
			return File.ReadAllText(path);
		}

		private Property ProcessField(Field field)
		{
			if (field.ForeignKey != null) return ProcessForeignKey(field);
			return ProcessBasicFieldElements(ProcessAnnotations(field), field);
		}

		private Property ProcessFieldViewModel(Field field)
		{
			return ProcessBasicFieldElements(field);
		}

		private Property ProcessBasicFieldElements(Property property, Field field)
		{
			var result = property;
			result.TypeProperty = field.Type;
			result.hasGeterAndSeter = true;
			result.Name = field.Name;
			return result;
		}
		private Property ProcessBasicFieldElements(Field field)
		{
			var result = new Property();
			result.TypeProperty = field.Type;
			result.hasGeterAndSeter = true;
			result.Name = field.Name;
			return result;
		}

		private Property ProcessForeignKey(Field field)
		{
			var result = new Property
			{
				Visibility = Visibility.Public,
				TypeProperty = (field.ForeignKey!.Relationship == "OneToOne")
					? $"{field.ForeignKey.ModelName} "
					: $"ICollection<{field.ForeignKey.ModelName}>? ",
				Name = field.ForeignKey.PropertyName
			};
			return result;
		}

		private Property ProcessAnnotations(Field field)
		{

			bool hasRequired = ((field.Required == null) ? false : true);
			bool hasStringLength = (field.StringLength != null) ? true : false;
			bool hasDataType = !string.IsNullOrEmpty(field.DataType);
			bool hasColumn = (field.Column == null) ? false : !string.IsNullOrEmpty(field.Column.TypeName);

			StringBuilder annotations = new StringBuilder();

			annotations.AppendLine((hasRequired) ? GetAnnotationRequired(field.Required!) : null);
			annotations.AppendLine((hasStringLength) ? GetAnnotationStringLength(field.StringLength!) : null);
			annotations.AppendLine((hasDataType) ? GetAnnotationDataType(field.DataType!) : null);
			annotations.AppendLine((hasColumn) ? GetAnnotationColumn(field.Column!.TypeName!) : null);

			var result = new Property
			{
				Annotations = annotations.ToString(),
			};

			return result;
		}

		private string GetAnnotationRequired(RequiredField requiredField)
		{
			if (string.IsNullOrEmpty(requiredField.ErrorMessage))
			{
				return "[Required]";
			}
			else
			{
				return $"[Required(ErrorMessage = \"{requiredField.ErrorMessage}\")]";
			}
		}

		private string GetAnnotationStringLength(StringLength stringLength)
		{
			if (stringLength.ErrorMessage != null &&
				stringLength.MaximumLength != null &&
				stringLength.MinimumLength != null)
			{
				return $"[StringLength({stringLength.MaximumLength}, ErrorMessage = \"{stringLength.ErrorMessage}\", MinimumLength = {stringLength.MinimumLength})]";
			}
			if (stringLength.ErrorMessage != null &&
				stringLength.MaximumLength != null)
			{
				return $"[StringLength({stringLength.MaximumLength}, ErrorMessage = \"{stringLength.ErrorMessage}\")]";
			}
			if (stringLength.ErrorMessage != null)
			{
				return $"[StringLength(ErrorMessage = \"{stringLength.ErrorMessage}\")]";
			}
			return "";
		}

		private string GetAnnotationDataType(string dataType)
		{
			return $"[DataType(DataType.{dataType})]";
		}

		private string GetAnnotationColumn(string typeName)
		{
			return $"[Column(TypeName = \"{typeName}\")]";
		}


		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("model", args);
		}
	}
}