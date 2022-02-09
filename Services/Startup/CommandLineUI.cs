
using Contracts.Interfaces;



namespace Services.Startup
{
	[AddService]
	public class CommandLineUI : ICommandLineUI
	{
		private readonly ICreateProjectService _createProjectService;
		private readonly IHelpService _helpService;

		public CommandLineUI(ICreateProjectService createProjectService, IHelpService helpService)
		{
			_createProjectService = createProjectService;
			_helpService = helpService;
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
					case "new":  return _createProjectService;
					case "help": return _helpService;
					default:
						return null;
				}
			}
			return null;
		}
	}
}