using Contracts.Interfaces;
using Services.Abstract;


namespace Services.Commands
{
	[AddService]
	public class EntityFrameworkService : AbstractService, IEntityFrameworkService
	{
		private readonly IShellCommandExecutor _shellCommandExecutor;

		public EntityFrameworkService(IShellCommandExecutor shellCommandExecutor)
		{
			_shellCommandExecutor = shellCommandExecutor;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			string command = args[1];

			string migrationName = "";
			if (args.Length > 2) migrationName = args[2];

			switch (command)
			{
				case "add-migration": return AddMigration(migrationName);
				case "remove-migration": return RemoveMigration();
				case "list-migration": return ListMigration();
				case "update-database": return UpdateDatabase();
				default:
					return -1;
			}

		}

		private int CheckInDirectory()
		{
			var directory = Path.GetFileName(CurrentDirectory);
			if (directory != "Api" && directory != "net6.0")
			{
				System.Console.WriteLine("Go to the folder Api and run this command again.");
				return -1;
			}
			return 1;
		}


		public int AddMigration(string migrationName)
		{
			if (CheckInDirectory() == -1) return -1;
			string program = $"dotnet";
			string args = $"ef migrations add {migrationName} --project Api.csproj --startup-project Api.csproj";

			var result = (_shellCommandExecutor.ExecuteCommand(program, args)) ? 1 : 0;
			return result;
		}

		public int RemoveMigration()
		{
			if (CheckInDirectory() == -1) return -1;
			string program = $"dotnet";
			string args = $"ef migrations remove";
			var result = (_shellCommandExecutor.ExecuteCommand(program, args)) ? 1 : 0;
			return result;
		}

		public int UpdateDatabase()
		{
			if (CheckInDirectory() == -1) return -1;
			string program = $"dotnet";
			string args = $"ef database update --project Api.csproj --startup-project Api.csproj";
			var result = (_shellCommandExecutor.ExecuteCommand(program, args)) ? 1 : 0;
			return result;
		}

		public int ListMigration()
		{
			if (CheckInDirectory() == -1) return -1;
			string program = $"dotnet";
			string args = $"ef migrations list";
			var result = (_shellCommandExecutor.ExecuteCommand(program, args)) ? 1 : 0;
			return result;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;

			return IsTheReservedWord("ef", args);
		}
	}
}