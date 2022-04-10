
using System.Collections.Immutable;
using Contracts.Interfaces;
using Services.Abstract;
using Models;
using System.Text;

namespace Services.Commands
{
	public partial class CreateServiceCrudService
	{

		private void WriteService(ImmutableList<string> models)
		{
			_codeGenerator
				.FileBuilder
					.WriteFile(GenService(models),
					$"{CurrentDirectory}/Services/"
					);
			System.Console.WriteLine($"GENERATED ../Services/{models.First()}Service.cs");
		}

		private FileCode GenService(ImmutableList<string> models)
		{
			var imports = new string[] {
				"AutoMapper",
				"Contracts.Repository",
				"Contracts.Service",
				"Entities.Models",
				"Entities.ViewModels",
				"Services.Abstract",
				"Microsoft.Extensions.Configuration",
				"System.Collections.Generic",
			}.ToImmutableList();

			var heritage = new string[]{
				"ServiceCrudAbstract",
				$"I{models.First()}Service"
			}.ToImmutableList();

			var properties = new Property[]{
				new Property {
					Name = $"_{models.First()}Repository",
					TypeProperty = $"readonly I{models.First()}Repository<{models.First()}>",
					Visibility = Visibility.Private
				}
			}.ToImmutableList();

			var result = _codeGenerator
				.ClassGenerator
					.CreateClass(
						imports,
						$"{models.First()}Service",
						"Services",
						GetMethods(models.First()),
						properties,
						heritage
					);

			return result;
		}
		// ---------------------------------Methods ---------------------------------------
		private ImmutableList<IMethodDefinition> GetMethods(string modelName)
		{
			return new IMethodDefinition[] {
				ConstructoWithBaseObjectAction(modelName),
				NewRecordMethod(modelName),
				UpdateRecordMethod(modelName),
				ListRecordMethod(modelName),
				SelectViewModelByIdRecordMethod(modelName),
				DeleteRecordMethod(modelName)
			}.ToImmutableList();
		}
		// ---------------------------------Methods ---------------------------------------
		private IMethodDefinition ConstructoWithBaseObjectAction(string modelName)
		{
			var baseObject = new string[]{
				"Mapper",
				"Configuration",
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = $"I{modelName}Repository<{modelName}>",
					Name = $"{modelName}Repository"
				},
				new Parameter{
					Type = "IMapper",
					Name = "Mapper"
				},
				new Parameter{
					Type = "IConfiguration",
					Name = "Configuration"
				},
			}.ToImmutableList();

			var constructor = _methodDefinition.Builder
			.Name($"{modelName}Service")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.LogiContent($"_{modelName}Repository = {modelName}Repository;")
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		private IMethodDefinition NewRecordMethod(string modelName)
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"{modelName}NewViewModel",
					Name = "ViewModel"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("NewRecord")
			.Parameters(parameters)
			.LogiContent(NewRecordMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "bool",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string NewRecordMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine("CleanErrors();");
			result.AppendLine($"var newRecord = _Mapper.Map<{modelName}>(ViewModel);");
			result.AppendLine($"return _{modelName}Repository.Insert(newRecord);");
			return result.ToString();
		}

		private IMethodDefinition UpdateRecordMethod(string modelName)
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"{modelName}UpdateViewModel",
					Name = "ViewModel"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("Update")
			.Parameters(parameters)
			.LogiContent(UpdateRecordMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "bool",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string UpdateRecordMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine("CleanErrors();");
			result.AppendLine($"var updateRecord = _Mapper.Map(ViewModel, _{modelName}Repository.SelectById(ViewModel.Id));");
			result.AppendLine($"return _{modelName}Repository.Update(updateRecord);");
			return result.ToString();
		}
		private IMethodDefinition DeleteRecordMethod(string modelName)
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = "int",
					Name = "id"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("Delete")
			.Parameters(parameters)
			.LogiContent(DeleteRecordMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "bool",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string DeleteRecordMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return _{modelName}Repository.Delete(id);");
			return result.ToString();
		}
		private IMethodDefinition ListRecordMethod(string modelName)
		{

			var result = _methodDefinition.Builder
			.Name("SelectAllViews")
			.LogiContent(ListRecordMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = $"List<{modelName}ViewModel>",
				Visibility = Visibility.Public,
			})
			.Create();
			return result;
		}

		private string ListRecordMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return _Mapper.Map<List<{modelName}ViewModel>>(_{modelName}Repository.SelectAll());");
			return result.ToString();
		}
		private IMethodDefinition SelectViewModelByIdRecordMethod(string modelName)
		{
			var parameters = new Parameter[]{
				new Parameter{
					Type = "int",
					Name = "Id"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("SelectOneViewById")
			.Parameters(parameters)
			.LogiContent(SelectByIdRecordMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = $"{modelName}ViewModel",
				Visibility = Visibility.Public,
			})
			.Create();
			return result;
		}

		private string SelectByIdRecordMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return _Mapper.Map<{modelName}ViewModel>(_{modelName}Repository.SelectById(Id));");
			return result.ToString();
		}
	}

}