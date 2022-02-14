using NUnit.Framework;
using Contracts.Interfaces;
using Services.Startup;
using Services.Tools;
using Models;
using Services;
using Moq;
using Services.Coder;
using System.Collections.Generic;
using System.Collections.Immutable;

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
				new string[] {"-g", "ssl"},
				new string[] {"generate", "ssl"},
				new string[] {"help"}
		};
		ICommandLineUI? _commandLineUI;
		IShellCommandExecutor? _shellCommandExecutor;

		ICreateProjectService? _createProjectService;
		ICreateSSLCertificateService? _createSSLCertificateService;

		IHelpService? _helpService;

		ICreateModelService? _createModelService;

		IClassDefinition? _classDefinition;
		IMethodDefinition? _methodDefinition;
		IBuilderClassDefinition? _builderClassDefinition;
		IBuilderMethodDefinition? _builderMethodDefinition;
		//IFileBuilder? _fileBuilder;

		[SetUp]
		public void Setup()
		{
			var mock = new Mock<IShellCommandExecutor>();
			mock.Setup(x => x.ExecuteCommand("ls", " -ln")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "new sln -n teste-api")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "dev-certs https --clean")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "dev-certs https --trust")).Returns(true);

			var code = new FileCode
			{
				Code = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
    static void Main(string[] args)
    {
    }
    }
}",
				FileName = "Novo.cs"
			};

			List<Models.FileCode> codes = new List<Models.FileCode>();
			codes.Add(code);

			var fileBuilderMock = new Mock<IFileBuilder>();
			fileBuilderMock.Setup(x => x.WriteFile(code, "/user/path")).Returns(true);
			fileBuilderMock.Setup(x => x.WriteFiles(codes.ToImmutableList(), "/user/path")).Returns(true);

			_shellCommandExecutor = mock.Object;

			_builderClassDefinition = new BuilderClassDefinition();
			_builderMethodDefinition = new BuilderMethodDefinition();

			_classDefinition = new ClassDefinition(_builderClassDefinition);
			_methodDefinition = new MethodDefinition(_builderMethodDefinition);
		
			_createProjectService = new CreateProjectService(_shellCommandExecutor);
			_helpService = new HelpService();
			_createSSLCertificateService = new CreateSSLCertificateService(_shellCommandExecutor);
			_createModelService = new CreateModelService(_classDefinition, _methodDefinition, fileBuilderMock.Object);

			_commandLineUI = new CommandLineUI(
				_createProjectService,
				_helpService,
				_createSSLCertificateService,
				_createModelService);
		}

		[Test]
		[TestCaseSource(nameof(CommandCases))]
		public void ExecuteCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			if (_commandLineUI != null)
			{
				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.Greater(result, 0);
			}
		}

		[Test]
		public void ShellCommandExecutor_Result_True()
		{
			if (_shellCommandExecutor != null)
			{
				var result = _shellCommandExecutor.ExecuteCommand("ls", " -ln");

				Assert.AreEqual(result, true);
			}
		}
	}
}