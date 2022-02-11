using System;
using Contracts.Interfaces;
using System.Linq;

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

		_shellCommandExecutor.ExecuteCommand("dotnet", $"new sln -n Solution{args[1]}");

		ExecuteCommandsInArray(GetProjects());
		ExecuteCommandsInArray(GetReferences("Api"), $"{currentDirectory}/Api");
		ExecuteCommandsInArray(GetReferences("Tests"), $"{currentDirectory}/Tests");
		ExecuteCommandsInArray(GetProjectsToSolution($"Solution{args[1]}"));

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
		"new classlib -o Models",
		"new classlib -o Services",
		"new classlib -o Tools"};
	}

	private string[] GetReferences(string folder)
	{
		string currentPath = Environment.CurrentDirectory;
		string[] refereces = new string[]{
		$"add reference {currentPath}/Contracts/Contracts.csproj",
		$"add reference {currentPath}/Models/Models.csproj",
		$"add reference {currentPath}/Services/Services.csproj",
		$"add reference {currentPath}/Tools/Tools.csproj"};
		return refereces;
	}

	private string[] GetProjectsToSolution(string solutionName)
	{
		return new string[]{
			$"sln {solutionName}.sln add Api/Api.csproj",
			$"sln {solutionName}.sln add Contracts/Contracts.csproj",
			$"sln {solutionName}.sln add Models/Models.csproj",
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

