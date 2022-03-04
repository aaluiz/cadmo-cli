using Contracts.Interfaces;
using Services.Abstract;

namespace Services.Commands
{
	[AddService]
	public class AddPackageService : AbstractService, IAddPackageService
	{
		private readonly IShellCommandExecutor _shellCommandExecutor;

		public AddPackageService(IShellCommandExecutor shellCommandExecutor)
        {
			_shellCommandExecutor = shellCommandExecutor;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			return AddPackage(GetPackageName(args));
		}

		private string GetPackageName(string[] args){
			if (args.Length == 4){
				return $"{args[1]} {args[2]} {args[3]}";
			}
			return $"{args[1]}";
		}

        private int AddPackage(string package){
			int result = 
				(_shellCommandExecutor
					.ExecuteCommand(
						"dotnet", 
						$"add package {package}")) 
						? 1: 0;
			return result;
		} 

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("add", args);
		}
	}
}