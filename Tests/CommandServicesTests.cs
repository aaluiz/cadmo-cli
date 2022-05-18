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
using Services.Commands.Tools;
using System;
using System.Reflection;
using System.IO;
using static Services.Commands.Tools.SyntaxAnaliser;

namespace Tests
{

	[TestFixture]
	public class CommandServicesTests
	{
		#region Properties And Cases
		static object[] AddPackageWorkCases = {
				new string[] {"add", "linq2db", "--version", "4.0.0-preview.10"},
				new string[] {"add", "linq2db"},
		};

		static object[] EntityFrameWorkCases = {
				new string[] {"ef", "add-migration", "initial" },
				new string[] {"ef", "remove-migration"},
				new string[] {"ef", "list-migration" },
				new string[] {"ef", "update-database"},
		};
		static object[] CommandCases = {
				new string[] {"g", "repository", "Categoria" },
				new string[] {"repository-di"},
				new string[] {"new", "App"},
				new string[] {"g", "ssl"},
		};
		static object[] CommandServices = {
				new string[] {"g", "service-crud", "--models", "ModelExample,Categoria"},
				new string[] {"generate", "service-crud", "--models", "ModelExample"},
				new string[] {"service-di"}
		};
		static object[] CommandControllers = {
				new string[] {"generate", "controller-crud", "--model", "ModelExample"},
				new string[] {"generate", "controller-crud", "--model", "ModelExample", "--secure"},
		};

		static object[] CommandCasesFails = {
				new string[] {"generate", "controller-crud", "--models", "ModelExample"},
				new string[] {"generate", "controller-crud", "--model", "ModelExample", "--secures"},
				new string[] {"g", "repository", "Categoriax" },
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
		IGenerateRepositoryExtensions? _generateRepositoryExtensions;
		private GenerateServiceExtensions? _generateServicesExtensions;
		IDirectoryHandler? _directoryHandler;
		private ICreateServiceCrudService? _createServiceCrudService;

		ICreateControllerService? _createControllerService;

		IEntityFrameworkService? _entityFrameworkService;
		IAddPackageService? _addPackageService;

		IVersionService? _versionService;

		#endregion

		#region  Setup
		[SetUp]
		public void Setup()
		{
			var mock = new Mock<IShellCommandExecutor>();
			mock.Setup(x => x.ExecuteCommand("ls", " -ln")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "new sln -n teste-api")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "dev-certs https --clean")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "dev-certs https --trust")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "ef migrations add initial --project Api.csproj --startup-project Api.csproj")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "ef migrations remove")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "ef migrations list")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "ef database update --project Api.csproj --startup-project Api.csproj")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("cd", "Api")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("cd", "..")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "add package linq2db --version 4.0.0-preview.10")).Returns(true);
			mock.Setup(x => x.ExecuteCommand("dotnet", "add package linq2db")).Returns(true);

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

			_createClassGenerator = new CreateClassGenerator(_classDefinition, _methodDefinition);

			_helpService = new HelpService();
			_createSSLCertificateService = new CreateSSLCertificateService(_shellCommandExecutor);
			_createInterfaceGenerator = new CreateInterfaceGenerator(_classDefinition, _methodDefinition);

			_codeGenerator = new CodeGenerator(_createClassGenerator, _createInterfaceGenerator, fileBuilderMock.Object, _builderClassDefinition);
			_autoMapperCommandService = new AutoMapperCommandService(_codeGenerator, _methodDefinition);
			_directoryHandler = new DirectoryHandler();
			_createProjectService = new CreateProjectService(_shellCommandExecutor, _codeGenerator, _directoryHandler, _autoMapperCommandService);
			_createModelService = new CreateModelService(_codeGenerator, _shellCommandExecutor);
			_buildCommandSevice = new BuildCommandService(_shellCommandExecutor);
			_serveCommandService = new ServeCommandService(_shellCommandExecutor);
			_dbContextCommandService = new DbContextCommandService(_codeGenerator, _methodDefinition);
			_createRepositoryService = new CreateRepositoryService(_codeGenerator, _methodDefinition);


			_generateRepositoryExtensions = new GenerateRepositoryExtensions(_codeGenerator, _methodDefinition, _directoryHandler);
			_generateServicesExtensions = new GenerateServiceExtensions(_codeGenerator, _methodDefinition, _directoryHandler);
			_createServiceCrudService = new CreateServiceCrudService(_codeGenerator, _methodDefinition, _directoryHandler);
			_createControllerService = new CreateControllerService(_codeGenerator, _methodDefinition, _directoryHandler);
			_entityFrameworkService = new EntityFrameworkService(_shellCommandExecutor);
			_addPackageService = new AddPackageService(_shellCommandExecutor);
			_versionService = new VersionService();

			_generateModelByScript = new GenerateModelByScript(
						_codeGenerator,
						_methodDefinition,
						_autoMapperCommandService,
						_dbContextCommandService,
						_shellCommandExecutor);
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
				_createRepositoryService,
				_generateRepositoryExtensions,
				_createServiceCrudService,
				_createControllerService,
				_entityFrameworkService,
				_addPackageService,
				_generateServicesExtensions,
				_versionService
				);
		}

		#endregion
		[Test]
		public void SystaxLab()
		{
			const string programText =
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }

		static void Method1(){};

		static void Method2(){};
    }
}";

			var result = GetMethods(programText);

			Assert.IsTrue(result.Count > 0);
		}

		[Test]
		[TestCaseSource(nameof(CommandServices))]
		public void ServiceCrudCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			if (_commandLineUI != null)
			{
				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.Greater(result, 0);
			}
		}

		[Test]
		[TestCaseSource(nameof(EntityFrameWorkCases))]
		public void EnttityFrameworkCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			if (_commandLineUI != null)
			{

				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.Greater(result, 0);
			}
		}

		[Test]
		[TestCaseSource(nameof(AddPackageWorkCases))]
		public void AddPackageCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			if (_commandLineUI != null)
			{

				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.Greater(result, 0);
			}
		}


		[Test]
		[TestCaseSource(nameof(CommandControllers))]
		public void ControllerCrudCommmand_ReturnExpect_MoreThanZero(string[] args)
		{
			if (_commandLineUI != null)
			{
				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.Greater(result, 0);
			}
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
		[TestCaseSource(nameof(CommandCasesFails))]
		public void Fail_ExecuteCommmand(string[] args)
		{
			if (_commandLineUI != null)
			{
				var result = _commandLineUI.ExecuteCommmand(args);

				Assert.AreEqual(result, -1);
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

		// [Test]
		// public void DirectoryHandler_ReturnFilesInFolderSevice()
		// {
		// 	var services = _directoryHandler!.GetServiceNames("/Users/alanluiz/lab/api/demo/");


		// 	services.ForEach((s) => { Console.WriteLine(s); });


		// 	Assert.Greater(services.Count, 1);
		// }

		// [Test]
		// public void DirectoryHandler_ReturnFilesInFolderRepositories()
		// {
		// 	var services = _directoryHandler!.GetRespoistoryNames("/Users/alanluiz/work/agendamento/backend");


		// 	services.ForEach((s) => { Console.WriteLine(s); });


		// 	Assert.Greater(services.Count, 1);
		// }

	}
}
