
using System.Collections.Immutable;
using Contracts.Interfaces;
using Services.Abstract;
using Models;
using System.Text;


namespace Services.Commands
{
	public partial class CreateControllerService
	{
		private void WriteController(string model, bool sercure)
		{
			_codeGenerator
				.FileBuilder
					.WriteFile(
						CreateController(model, sercure),
						$"{CurrentDirectory}/API/Controllers/"
					);
			System.Console.WriteLine($"GENERATED ../Api/Controllers/{model}Controller.cs");
		}

		FileCode CreateController(string model, bool sercure)
		{
			var annotations = new StringBuilder();

			annotations.AppendLine($"[Route(\"api/[controller]\")]");
			annotations.AppendLine($"[ApiController]");
			annotations.AppendLine((sercure) ? $"[Authorize]" : "");
			annotations.AppendLine("\n");

			var imports = new string[] {
				"Contracts",
				"Contracts.Service",
				"Entities.Models",
				"Entities.ViewModels",
				"Api.Controllers.Abstract",
				"Api.Controllers.ActionFilters",
				"Microsoft.AspNetCore.Authorization",
				"Microsoft.AspNetCore.Http",
				"Microsoft.AspNetCore.Mvc",
				"System.Net.Mime",
			}.ToImmutableList();

			var heritage = new string[]{
				"ControllerAbstract",
			}.ToImmutableList();

			var properties = new Property[]{
				new Property {
					Name = $"_{model}Service",
					TypeProperty = $"readonly I{model}Service",
					Visibility = Visibility.Private
				}
			}.ToImmutableList();

			var codeBuilder = _codeGenerator
				.BuilderCustonClass
					.Annotations(annotations.ToString())
					.Imports(imports)
					.Properties(properties)
					.Name($"{model}Controller")
					.Namespace("Api.Controllers")
					.Inheritance(heritage)
					.Methods(GetMethods(model))
					.Create();

			return new FileCode
			{
				FileName = $"{model}Controller.cs",
				Code = codeBuilder.ClassCode
			};
		}

		// --------------------------Methods------------------------------------
		private ImmutableList<IMethodDefinition> GetMethods(string modelName)
		{
			return new IMethodDefinition[] {
				ConstructoWithBaseObjectAction(modelName),
				GetMethod(modelName),
				GetMethodById(modelName),
				PostMethod(modelName),
				PutMethod(modelName),
				DeleteMethod(modelName)
			}.ToImmutableList();
		}
		// -------------------------Methods------------------------------------End

		private IMethodDefinition ConstructoWithBaseObjectAction(string modelName)
		{
			var baseObject = new string[]{
				"Logger",
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = $"I{modelName}Service",
					Name = $"{modelName}Service"
				},
				new Parameter{
					Type = "ILoggerManager",
					Name = "Logger"
				},
			}.ToImmutableList();

			var constructor = _methodDefinition.Builder
			.Name($"{modelName}Controller")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.LogiContent($"_{modelName}Service = {modelName}Service;")
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		// ------------------------------ GET -------------------------------
		private IMethodDefinition GetMethod(string modelName)
		{
			var annotations = new List<string>();

			annotations.Add($"HttpGet");
			annotations.Add($"ProducesResponseType(StatusCodes.Status200OK)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status404NotFound)\n");

			var result = _methodDefinition.Builder
			.Annotations(annotations.ToImmutableList())
			.Name("Get")
			.LogiContent(GetMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "IActionResult",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string GetMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"var data = _{modelName}Service.SelectAllViews();");
			result.AppendLine("if (data?.Count > 0)");
			result.AppendLine("return Ok(data);");
			result.AppendLine("return NotFound();");
			return result.ToString();
		}
		// -------------------------------------------------------------END GET



		// ------------------------------ GET/ID -----------------------------
		private IMethodDefinition GetMethodById(string modelName)
		{
			var annotations = new List<string>();

			annotations.Add("HttpGet(\"{id}\")");
			annotations.Add($"ProducesResponseType(StatusCodes.Status200OK)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status404NotFound)\n");
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"int",
					Name = "id"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Annotations(annotations.ToImmutableList())
			.Name("Get")
			.Parameters(parameters)
			.LogiContent(GetByIdMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "IActionResult",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string GetByIdMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"var data = _{modelName}Service.SelectOneViewById(id);");
			result.AppendLine("if (data != null)");
			result.AppendLine("return Ok(data);");
			result.AppendLine("return NotFound();");
			return result.ToString();
		}
		// --------------------------------------------------------------END GET/DI


		// ----------------------------------- POST -------------------------------
		private IMethodDefinition PostMethod(string modelName)
		{
			var annotations = new List<string>();

			annotations.Add($"HttpPost");
			annotations.Add("Consumes(MediaTypeNames.Application.Json)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status200OK)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status404NotFound)");
			annotations.Add("ServiceFilter(typeof(ValidationFilterAttribute))\n");
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"[FromBody] {modelName}NewViewModel",
					Name = "NewRecord"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("Post")
			.Annotations(annotations.ToImmutableList())
			.Parameters(parameters)
			.LogiContent(PostMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "IActionResult",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string PostMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return ResponseAction(_{modelName}Service.NewRecord(NewRecord),");
			result.AppendLine($"_{modelName}Service.HasErro,");
			result.AppendLine($"_{modelName}Service.ErrorMessages);");
			return result.ToString();
		}
		// --------------------------------------------------------------------END POST


		// ------------------------------- PUT --------------------------------------
		private IMethodDefinition PutMethod(string modelName)
		{
			var annotations = new List<string>();

			annotations.Add("HttpPut(\"{id}\")");
			annotations.Add("Consumes(MediaTypeNames.Application.Json)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status200OK)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status404NotFound)");
			annotations.Add("ServiceFilter(typeof(ValidationFilterAttribute))\n");
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"[FromBody] {modelName}UpdateViewModel",
					Name = "Record"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("Put")
			.Annotations(annotations.ToImmutableList())
			.Parameters(parameters)
			.LogiContent(PutMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "IActionResult",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string PutMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return ResponseAction(_{modelName}Service.Update(Record),");
			result.AppendLine($"_{modelName}Service.HasErro,");
			result.AppendLine($"_{modelName}Service.ErrorMessages);");
			return result.ToString();
		}

		// ---------------------------------------------------------------------END PUT

		// ----------------------------- DELETE --------------------------------------------
		private IMethodDefinition DeleteMethod(string modelName)
		{
			var annotations = new List<string>();

			annotations.Add("HttpDelete(\"{id}\")");
			annotations.Add("Consumes(MediaTypeNames.Application.Json)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status200OK)");
			annotations.Add($"ProducesResponseType(StatusCodes.Status404NotFound)");
			annotations.Add("ServiceFilter(typeof(ValidationFilterAttribute))\n");
			var parameters = new Parameter[]{
				new Parameter{
					Type = $"int",
					Name = "id"
				},
			}.ToImmutableList();

			var result = _methodDefinition.Builder
			.Name("Delete")
			.Annotations(annotations.ToImmutableList())
			.Parameters(parameters)
			.LogiContent(DeleteMethodLogic(modelName))
			.ReturnDefinition(new ReturnDefinition
			{
				Type = "ActionResult<bool>",
				Visibility = Visibility.Public
			})
			.Create();
			return result;
		}

		private string DeleteMethodLogic(string modelName)
		{
			var result = new StringBuilder();
			result.AppendLine($"return _{modelName}Service.Delete(id);");
			return result.ToString();
		}

	}

}