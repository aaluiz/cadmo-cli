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
	public partial class GenerateModelByScript : AbstractService, IGenerateModelByScript
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;
		private readonly IAutoMapperCommandService _autoMapperCommandService;
		private readonly IDbContextCommandService _dbContextCommandService;
		private readonly IShellCommandExecutor _shellCommandExecutor;

		public GenerateModelByScript(
			ICodeGenerator codeGenerator,
			IMethodDefinition methodDefinition,
			IAutoMapperCommandService autoMapperCommandService,
			IDbContextCommandService dbContextCommandService,
			IShellCommandExecutor shellCommandExecutor)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
			_autoMapperCommandService = autoMapperCommandService;
			_dbContextCommandService = dbContextCommandService;
			_shellCommandExecutor = shellCommandExecutor;

		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			switch (args[2])
			{
				case "--with-script": return GenerateModel(args[3], false);
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
			GetModels($"{CurrentDirectory}/Entities/JsonModelsDefinition")
				.ForEach((model) =>
				{
					GenerateModel(model, false);
				});

			return 1;
		}

		private int SafetyGenerate()
		{
			GetModels($"{CurrentDirectory}/Entities/JsonModelsDefinition")
				.ForEach((model) =>
				{
					GenerateModel(model, true);
				});

			return 1;
		}

		private int GenerateModel(string scriptModelName, bool safe)
		{
			SaveFileOnDisk(GeneraBasicModelCode(scriptModelName), "Models", safe);

			SaveFileOnDisk(GenerateViewModel(scriptModelName), $"ViewModels/{scriptModelName}", safe);
			SaveFileOnDisk(GenerateViewModelUpdateOrNew(scriptModelName, true), $"ViewModels/{scriptModelName}", safe);
			SaveFileOnDisk(GenerateViewModelUpdateOrNew(scriptModelName, false), $"ViewModels/{scriptModelName}", safe);
			if (!safe)
			{
				_autoMapperCommandService.Execute(new string[] { "g", "automapper" });
				_dbContextCommandService.Execute(new string[] { "g", "dbcontext" });
			}
			return 1;
		}

		private List<string> GetModels(string path)
		{
			var resutl = Directory
				.GetFiles(path).ToList()
				.Select(x => Path.GetFileNameWithoutExtension(x))
					.Where(x => x != "model.schema").ToList();
			return resutl;
		}

		private void SaveFileOnDisk(FileCode fileCode, string path, bool safe)
		{
			if (safe)
			{
				if (File.Exists($"{CurrentDirectory}/Entities/{path}/{fileCode.FileName}"))
				{
					System.Console.WriteLine($"{fileCode.FileName} already exits, if you wish overwrite it use option --force");
				}
			}
			else
			{
				_codeGenerator
					.FileBuilder!
					.WriteFile(fileCode, $"{CurrentDirectory}/Entities/{path}");
				System.Console.WriteLine($"GENERATED {CurrentDirectory}/Entities/{path}/{fileCode.FileName}");
			}

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


		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("model", args);
		}
	}
}