using Contracts.Interfaces;
using Services.Abstract;

namespace Services.Commands
{
	[AddService]
	public class ServeCommandService : AbstractService, IServeCommandService
	{
		private readonly IShellCommandExecutor _shellCommandExecutor;
		public ServeCommandService(IShellCommandExecutor shellCommandExecutor)
		{
			_shellCommandExecutor = shellCommandExecutor;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			var exitEvent = new ManualResetEvent(false);

			Console.CancelKeyPress += (sender, eventArgs) =>
			{
				eventArgs.Cancel = true;
				exitEvent.Set();
			};

			_shellCommandExecutor.ExecuteCommand("dotnet", "run --project Api");
			System.Console.WriteLine("Press Ctrl+C to shutdown.");

			exitEvent.WaitOne();

			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{

			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("serve", args);
		}
	}
}