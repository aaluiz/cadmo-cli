

using Services.Abstract;
using Contracts.Interfaces;

namespace Services
{
	[AddService]
	public class BuildCommandService : AbstractService, IBuildCommandService
	{
		IShellCommandExecutor _shellCommandExecutor;
		public BuildCommandService(IShellCommandExecutor shellCommandExecutor)
		{
			_shellCommandExecutor = shellCommandExecutor;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			_shellCommandExecutor.ExecuteCommand("dotnet", "build");
			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("build", args);
		}
	}

}