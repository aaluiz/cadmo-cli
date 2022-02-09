using System;
using Contracts.Interfaces;
using System.Linq;

[AddService]
public class CreateProjectService : AbstractService, ICreateProjectService
{
	string[] projects = new string[]{
		"new web -o Api",
		"new nunit -o Tests",
		"new classlib -o Contracts",
		"new classlib -o Models",
		"new classlib -o Services",
		"new classlib -o Tools",
	};

	string[] refereces = new string[]{
		"add reference ../Contracts/Contracts.csproj",
		"add reference ../Models/Models.csproj",
		"add reference ../Services/Services.csproj",
		"add reference ../Tools/Tools.csproj",
	};


	private readonly IShellCommandExecutor _shellCommandExecutor;
	public CreateProjectService(IShellCommandExecutor shellCommandExecutor)
	{
		_shellCommandExecutor = shellCommandExecutor;
	}
	public int Execute(string[] args)
	{
		if (!IsValidArgs(args)) return -1;

		if (args[0] != "new") return -1;

		var path = Environment.CurrentDirectory;

		_shellCommandExecutor.ExecuteCommand("dotnet", $"new sln -n Solution{args[1]}");

		foreach (string arg in projects)
		{
			_shellCommandExecutor.ExecuteCommand("dotnet", arg);
		}

		_shellCommandExecutor.ExecuteCommand("cd", "Api");

		foreach (string arg in refereces)
		{
			_shellCommandExecutor.ExecuteCommand("dotnet", arg);
		}
		_shellCommandExecutor.ExecuteCommand("cd", "..");
		_shellCommandExecutor.ExecuteCommand("cd", "Test");
		foreach (string arg in refereces)
		{
			_shellCommandExecutor.ExecuteCommand("dotnet", arg);
		}

		return 1;
	}
}

