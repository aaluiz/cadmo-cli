using System;
using System.Collections.Immutable;
using System.Reflection;
using Contracts.Interfaces;
using Models;
using Services.Abstract;
using Services.Commands.Tools;

[AddService]
public class CreateProjectService : AbstractService, ICreateProjectService
{
	string path = Environment.SystemDirectory;
	private readonly IShellCommandExecutor _shellCommandExecutor;
	private readonly ICodeGenerator _codeGenerator;
	public CreateProjectService(IShellCommandExecutor shellCommandExecutor, ICodeGenerator codeGenerator)
	{
		_shellCommandExecutor = shellCommandExecutor;
		_codeGenerator = codeGenerator;
	}
	public int Execute(string[] args)
	{
		if (!ValidateArgs(args)) return -1;

		string currentDirectory = Environment.CurrentDirectory;
		string apiDiretory = $"{currentDirectory}/Api";
		string testsDiretory = $"{currentDirectory}/Tests";
		string entitiessDiretory = $"{currentDirectory}/Entities";
		string servicesDiretory = $"{currentDirectory}/Services";
		string toolsDiretory = $"{currentDirectory}/Tools";
		string repositoriesDiretory = $"{currentDirectory}/Repositories";

		_shellCommandExecutor.ExecuteCommand("dotnet", $"new sln -n Solution{args[1]}");

		ExecuteCommandsInArray(GetProjects());

		ExecuteCommandsInArray(PackagesCommands.PackagesForApi(), apiDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForEntities(), entitiessDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForRepository(), repositoriesDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForServices(), servicesDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForTools(), toolsDiretory);

		ExecuteCommandsInArray(GetReferences("Api"), apiDiretory);
		ExecuteCommandsInArray(GetReferences("Tests"), testsDiretory);
		ExecuteCommandsInArray(GetProjectsToSolution($"Solution{args[1]}"));

		CreateDiretory("Entities", GetEntitiesDiretories());
		CreateDiretory("Contracts", GetContractDirectories());
		_codeGenerator.FileBuilder.WriteFile(RepositoryAbstract(), $"{CurrentDirectory}/Contracts/Repository/Abstract");
		_codeGenerator.FileBuilder.WriteFile(CodesForNewProject.GetLoggerFile(), $"{CurrentDirectory}/Contracts/Logger");
		WriteServiceInterfaces();
		GeneratedEntitiesIntefaces();
		WriteJsonSchemaModel();
		return 1;
	}

	public void WriteJsonSchemaModel()
	{
		string schema = File.ReadAllText($"{GetAssemblyPath()}/Assets/model.schema.json");
		var schemeFile = new FileCode
		{
			Code = schema,
			FileName = "model.schema.json"
		};

		string modelJson = File.ReadAllText($"{GetAssemblyPath()}/Assets/models.json");
		var models = new FileCode
		{
			Code = modelJson,
			FileName = "ModelExample.json"
		};

		var codes = new FileCode[] { schemeFile, models }.ToImmutableList();

		_codeGenerator.FileBuilder
			.WriteFiles(codes, $"{CurrentDirectory}/Entities/JsonModelsDefinition");

		System.Console.WriteLine("GENERATED Json Models Definitions Files");
	}

	public string GetAssemblyPath()
	{
		string codeBase = Assembly.GetExecutingAssembly().Location;
		return Path.GetDirectoryName(codeBase)!;
	}

	private void WriteServiceInterfaces()
	{
		CodesForNewProject.ServiceAbstractCodes().ForEach(x =>
			_codeGenerator
				.FileBuilder
				.WriteFile(x, $"{CurrentDirectory}/Contracts/Service/Abstract"));
	}

	override protected bool ValidateArgs(string[] args)
	{
		if (!IsValidArgs(args)) return false;
		return IsTheReservedWord("new", args);
	}

	private void GeneratedEntitiesIntefaces()
	{
		var imports = new string[] { "System" }.ToImmutableList();
		var properties = new Property[] { new Property {
			Name = "Id",
			TypeProperty = "int",
			Visibility = Visibility.Public,
			hasGeterAndSeter = true
		}}.ToImmutableList();

		var IEntity = _codeGenerator.InterfaceGenerator
		.CreateInterface(imports, "IEntity", "Entities.Interface", properties);

		var IEntityViewModel = _codeGenerator.InterfaceGenerator
		.CreateInterface("IEntityViewModel", "Entities.Interface");

		_codeGenerator.FileBuilder.WriteFile(IEntity, $"{CurrentDirectory}/Entities/Interface");
		_codeGenerator.FileBuilder.WriteFile(IEntityViewModel, $"{CurrentDirectory}/Entities/Interface");
		System.Console.WriteLine("GENERATED Entities/Interface files");
	}

	private string[] GetProjects()
	{
		return new string[]{
		"new webapi -o Api",
		"new nunit -o Tests",
		"new classlib -o Contracts",
		"new classlib -o Entities",
		"new classlib -o Repositories",
		"new classlib -o Services",
		"new classlib -o Tools"};
	}

	private ImmutableList<string> GetEntitiesDiretories()
	{
		var result = new List<string>();
		result.Add("AutoMapper");
		result.Add("Data");
		result.Add("Interface");
		result.Add("Models");
		result.Add("Models/Enums");
		result.Add("ViewModels");
		result.Add("JsonModelsDefinition");
		return result.ToImmutableList();
	}
	private ImmutableList<string> GetContractDirectories()
	{
		var result = new List<string>();
		result.Add("Repository");
		result.Add("Repository/Abstract");
		result.Add("Service");
		result.Add("Logger");
		result.Add("Service/Abstract");
		return result.ToImmutableList();
	}

	private FileCode RepositoryAbstract()
	{
		var result = new FileCode();

		result.Code = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contratos.Repository.Abstract
{
    public interface IRepository<Model>
    {
        IEnumerable<Model> SelectAll();

        Model SelectById(int RecordId);

        IQueryable<Model> SelectByProperty(Expression<Func<Model, bool>> predicate);

        bool Insert(Model NewModel);

        Task<bool> InsertAsync(Model NewModel);

        bool Update(Model ObjectModel);

        Task<bool> UpdateAsync(Model ObjectModel);

        bool Delete(int Id);

        Task<bool> DeleteAsync(int Id);
    }
}
";
		result.FileName = "IRepository.cs";
		return result;
	}

	private void CreateDiretory(string projectName, ImmutableList<string> directory)
	{
		string currentPath = Environment.CurrentDirectory;
		directory.ForEach(x => Directory.CreateDirectory($"{currentPath}/{projectName}/{x}"));
	}

	private string[] GetReferences(string folder)
	{
		string currentPath = Environment.CurrentDirectory;
		string[] refereces = new string[]{
		$"add reference {currentPath}/Contracts/Contracts.csproj",
		$"add reference {currentPath}/Entities/Entities.csproj",
		$"add reference {currentPath}/Services/Services.csproj",
		$"add reference {currentPath}/Tools/Tools.csproj"};
		return refereces;
	}

	private string[] GetProjectsToSolution(string solutionName)
	{
		return new string[]{
			$"sln {solutionName}.sln add Api/Api.csproj",
			$"sln {solutionName}.sln add Contracts/Contracts.csproj",
			$"sln {solutionName}.sln add Entities/Entities.csproj",
			$"sln {solutionName}.sln add Services/Services.csproj",
			$"sln {solutionName}.sln add Tools/Tools.csproj",
			$"sln {solutionName}.sln add Tests/Tests.csproj",
	  };
	}


	private void ExecuteCommandsInArray(string[] commands, string? workingPath = null)
	{
		foreach (string command in commands)
		{
			if (workingPath != null)
				_shellCommandExecutor.ExecuteCommand("dotnet", command, workingPath);
			else
				_shellCommandExecutor.ExecuteCommand("dotnet", command);

			WaitOneSecond();
		}
	}

	private void SetTrustCertificate()
	{
		_shellCommandExecutor.ExecuteCommand("dotnet", "dev-certs https --clean");
		_shellCommandExecutor.ExecuteCommand("dotnet", "dev-certs https --trust");
	}

	private void WaitOneSecond()
	{
		Thread.Sleep(1000);
	}
}

