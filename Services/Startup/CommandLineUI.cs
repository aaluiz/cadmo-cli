
using Contracts.Interfaces;



namespace Services.Startup
{
	[AddService]
	public class CommandLineUI : ICommandLineUI
	{
		private readonly ICreateProjectService _createProjectService;
		private readonly IHelpService _helpService;
		private readonly ICreateSSLCertificateService _createSSLCertificateService;
		private readonly ICreateModelService _createModelService;
		private readonly IBuildCommandService _buildCommandSevice;
		private readonly IServeCommandService _serveCommandService;

		private readonly IDbContextCommandService _dbContextCommandService;

		private readonly IAutoMapperCommandService _autoMapperCommandService;
		private readonly IGenerateModelByScript _generateModelByScript;
		private readonly ICreateRepositoryService _createRepositoryService;
		private readonly IGenerateRepositoryExtensions _generateRepositoyExtensions;

		public CommandLineUI(ICreateProjectService createProjectService,
			IHelpService helpService,
			ICreateSSLCertificateService createSSLCertificateService,
			ICreateModelService createModelService,
			IBuildCommandService buildCommandSevice,
			IServeCommandService serveCommandService,
			IAutoMapperCommandService autoMapperCommandService,
			IDbContextCommandService dbContextCommandService,
			IGenerateModelByScript generateModelByScript,
			ICreateRepositoryService createRepositoryService,
			IGenerateRepositoryExtensions repositoryExtensions)
		{
			_createProjectService = createProjectService;
			_helpService = helpService;
			_createSSLCertificateService = createSSLCertificateService;
			_createModelService = createModelService;
			_buildCommandSevice = buildCommandSevice;
			_serveCommandService = serveCommandService;
			_autoMapperCommandService = autoMapperCommandService;
			_dbContextCommandService = dbContextCommandService;
			_generateModelByScript = generateModelByScript;
			_createRepositoryService = createRepositoryService;
			_generateRepositoyExtensions = repositoryExtensions;
		}

		public int ExecuteCommmand(string[] args)
		{
			var command = GetCommand(args);
			return (command != null) ? command.Execute(args) : -1;
		}

		private ICommand? GetCommand(string[] args)
		{
			if (args.Length != 0)
			{
				switch (args[0])
				{
					case "new": return _createProjectService;
					case "help": return _helpService;
					case "build": return _buildCommandSevice;
					case "serve": return _serveCommandService;
					case "repository-di": return _generateRepositoyExtensions;
					case "g": return GetService(args);
					case "generate": return GetService(args);
					default:
						return null;
				}
			}
			return null;
		}

		private ICommand? GetService(string[] args)
		{
			switch (args.Length)
			{	case 2: 
					switch (args[1])
					{
						case "ssl": return _createSSLCertificateService;
						case "automapper": return _autoMapperCommandService;
						case "dbcontext": return _dbContextCommandService;
						default:
							return null;
					};
				
				case 3:
					switch (args[1])
					{
						case "ssl": return _createSSLCertificateService;
						case "model": return _createModelService;
						case "repository": return _createRepositoryService;
						case "automapper": return _autoMapperCommandService;
						case "dbcontext": return _dbContextCommandService;
						default:
							return null;
					};
				case 4:
					switch (args[1])
					{
						case "model":
							switch (args[2])
							{
								case "--with-script": return _generateModelByScript;
								case "--with-all-scripts": return _generateModelByScript;
								default:
									return null;
							}
						default:
							return null;
					}
				default:
					return null;
			}
		}
	}
}