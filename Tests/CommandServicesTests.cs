using NUnit.Framework;
using Contracts.Interfaces;
using Services.Startup;
using Services.Tools;
using Services;
using Moq;

namespace Tests
{

	[TestFixture]
	public class CommandServicesTests
	{
		static object[] CommandCases = {
				new string[] {"new", "teste-api"},
				new string[] {"-g", "model", "User" },
				new string[] {"-g", "repository", "User" },
				new string[] {"-g", "controller", "User" },
				new string[] {"-g", "service", "User" },
				new string[] {"-g", "model", "User" },
				new string[] {"generate", "model", "User" },
				new string[] {"generate", "repository", "User" },
				new string[] {"generate", "service", "User" },
				new string[] {"generate", "controller", "User" },
				new string[] {"create", "endpoints"},
				new string[] {"help"}
		};
		ICommandLineUI _commandLineUI;
		IShellCommandExecutor _shellCommandExecutor;

		ICreateProjectService _createProjectService;

		IHelpService _helpService;

		[SetUp]
		public void Setup()
		{
			var mock = new Mock<IShellCommandExecutor>();
			mock.Setup(x => x.ExecuteCommand("ls", " -ln")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "new sln -n teste-api")).Returns(true);

			_shellCommandExecutor = mock.Object;


			_createProjectService = new CreateProjectService(_shellCommandExecutor);
			_helpService = new HelpService();
			_commandLineUI = new CommandLineUI(_createProjectService, _helpService);
		}

		[Test]
		[TestCaseSource(nameof(CommandCases))]
		public void ExecuteCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			var result = _commandLineUI.ExecuteCommmand(args);

			Assert.Greater(result, 0);
		}

		[Test]
		public void ShellCommandExecutor_Result_True()
		{

			var result = _shellCommandExecutor.ExecuteCommand("ls", " -ln");

			Assert.AreEqual(result, true);
		}
	}
}