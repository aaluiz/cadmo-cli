using NUnit.Framework;
using Services.Startup;
using Services.Tools;
using Contracts.Interfaces;

namespace Tests;

[TestFixture]
public class CommandServicesTests
{
	static object[] CommandCases = {
				new string[] {"cadmo-cli", "new", "teste-api"},
				new string[] {"cadmo-cli", "-g", "model", "User" },
				new string[] {"cadmo-cli", "-g", "repository", "User" },
				new string[] {"cadmo-cli", "-g", "controller", "User" },
				new string[] {"cadmo-cli", "-g", "service", "User" },
				new string[] {"cadmo-cli", "-g", "model", "User" },
				new string[] {"cadmo-cli", "generate", "model", "User" },
				new string[] {"cadmo-cli", "generate", "repository", "User" },
				new string[] {"cadmo-cli", "generate", "service", "User" },
				new string[] {"cadmo-cli", "generate", "controller", "User" },
								new string[] {"cadmo-cli", "create", "endpoints"}
		};
	ICommandLineUI _commandLineUI;
	IShellCommandExecutor _shellCommandExecutor;
	
	[SetUp]
	public void Setup()
	{
		_commandLineUI = new CommandLineUI();
		_shellCommandExecutor = new ShellCommandExecutor();
	}

	[Test]
	[TestCaseSource(nameof(CommandCases))]
	public void ExecuteCommmand_ReturnExpect_MoreThanZero(string[] args)
	{
		var result = _commandLineUI.ExecuteCommmand(args);

		Assert.Greater(result, 0);
	}

	[Test]
	public void ShellCommandExecutor_Result_True(){

		var result = _shellCommandExecutor.ExecuteCommand("ls", " -ln");

		Assert.AreEqual(result, true);
	}
}