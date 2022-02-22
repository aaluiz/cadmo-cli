using NUnit.Framework;
using Contracts.Interfaces;
using Services.Startup;
using Services.Commands;
using Models;
using Services;
using Moq;
using Services.Coder;
using System.Collections.Generic;
using System.Collections.Immutable;
using Services.Generators;

namespace Tests
{

	[TestFixture]
	public class CommandServicesTests
	{
		static object[] CommandCases = {
			//	new string[] {"g", "model", "User" },
				new string[] {"g", "repository", "Categoria" },
			//	new string[] {"g", "controller", "User" },
			//	new string[] {"g", "service", "User" },
		};

		static object[] ModelCommandCases = {
				new string[] {"g", "model", "User" },
				new string[] {"g", "model", "--with-script", "Categoria" },
				new string[] {"generate", "model", "--with-script", "Categoria" },
				new string[] {"g", "model", "--with-all-scripts", "--force" },
				new string[] {"g", "model", "--with-all-scripts", "--safety" },
				new string[] {"generate", "model", "--with-all-scripts", "--force" },
				new string[] {"generate", "model", "--with-all-scripts", "--safety" },
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
		ICodeGenerator? _codeGenerator;
		ICreateClassGenerator? _createClassGenerator;
		ICreateInterfaceGenerator? _createInterfaceGenerator;
		IBuildCommandService? _buildCommandSevice;
		//IFileBuilder? _fileBuilder;
		IServeCommandService? _serveCommandService;

		IAutoMapperCommandService? _autoMapperCommandService;
		IDbContextCommandService? _dbContextCommandService;

		IGenerateModelByScript? _generateModelByScript;
		ICreateRepositoryService? _createRepositoryService;
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


			_helpService = new HelpService();
			_createSSLCertificateService = new CreateSSLCertificateService(_shellCommandExecutor);
			_createClassGenerator = new CreateClassGenerator(_classDefinition, _methodDefinition);
			_createInterfaceGenerator = new CreateInterfaceGenerator(_classDefinition, _methodDefinition);

			_codeGenerator = new CodeGenerator(_createClassGenerator, _createInterfaceGenerator, fileBuilderMock.Object);
			_createProjectService = new CreateProjectService(_shellCommandExecutor, _codeGenerator);
			_createModelService = new CreateModelService(_codeGenerator);
			_buildCommandSevice = new BuildCommandService(_shellCommandExecutor);
			_serveCommandService = new ServeCommandService(_shellCommandExecutor);
			_autoMapperCommandService = new AutoMapperCommandService(_codeGenerator, _methodDefinition);
			_dbContextCommandService = new DbContextCommandService(_codeGenerator, _methodDefinition);
			_createRepositoryService = new CreateRepositoryService();

			_generateModelByScript = new GenerateModelByScript(
						_codeGenerator, 
						_methodDefinition,
						_autoMapperCommandService,
						_dbContextCommandService);
			_commandLineUI = new CommandLineUI(
				_createProjectService,
				_helpService,
				_createSSLCertificateService,
				_createModelService,
				_buildCommandSevice,
				_serveCommandService,
				_autoMapperCommandService,
				_dbContextCommandService,
				_generateModelByScript,
				_createRepositoryService);
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

		[TestCaseSource(nameof(ModelCommandCases))]
		public void ExecuteCommmand_Models_ReturnExpect_MoreThanZero(string[] args)
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