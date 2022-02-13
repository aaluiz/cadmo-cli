
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

		public CommandLineUI(ICreateProjectService createProjectService, IHelpService helpService, ICreateSSLCertificateService createSSLCertificateService, ICreateModelService createModelService)
		{
			_createProjectService = createProjectService;
			_helpService = helpService;
			_createSSLCertificateService = createSSLCertificateService;
			_createModelService = createModelService;
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
					case "-g": return GetService(args);
					case "generate": return GetService(args);
					default:
						return null;
				}
			}
			return null;
		}

		private ICommand? GetService(string[] args)
		{
			switch (args[1])
			{
				case "ssl": return _createSSLCertificateService;
				case "model": return _createModelService;
				default:
				return null;
			}
		}
	}
}