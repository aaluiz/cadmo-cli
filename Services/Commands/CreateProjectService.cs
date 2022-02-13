using System;
using System.Collections.Immutable;
using Contracts.Interfaces;
using Services.Commands.Tools;

[AddService]
public class CreateProjectService : AbstractService, ICreateProjectService
{
	string path = Environment.SystemDirectory;
	private readonly IShellCommandExecutor _shellCommandExecutor;
	public CreateProjectService(IShellCommandExecutor shellCommandExecutor)
	{
		_shellCommandExecutor = shellCommandExecutor;
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
		ExecuteCommandsInArray(GetReferences("Api"), apiDiretory);

		ExecuteCommandsInArray(GetReferences("Tests"), testsDiretory);
		ExecuteCommandsInArray(GetProjectsToSolution($"Solution{args[1]}"));

		ExecuteCommandsInArray(PackagesCommands.PackagesForApi(), apiDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForEntities(), entitiessDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForRepository(), repositoriesDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForServices(), servicesDiretory);
		ExecuteCommandsInArray(PackagesCommands.PackagesForTools(), toolsDiretory);

		CreateDiretory("Entities", GetEntitiesDiretorys());
		return 1;
	}

	override protected bool ValidateArgs(string[] args)
	{
		if (!IsValidArgs(args)) return false;
		return IsTheReserverWord("new", args);
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

	private ImmutableList<string> GetEntitiesDiretorys(){
		var result = new List<string>();
		result.Add("AutoMapper");
		result.Add("Data");
		result.Add("Interface");
		result.Add("Models");
		result.Add("Models/Enums");
		result.Add("ViewModels");
		return result.ToImmutableList();
	}

	private void CreateDiretory(string projectName, ImmutableList<string> directory){
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

