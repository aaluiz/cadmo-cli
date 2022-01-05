
using Contracts.Interfaces;



namespace Services.Startup
{
	[AddService]
	public class CommandLineUI : ICommandLineUI
	{
		public int ExecuteCommmand(string[] args)
		{
			return 1;
		}
	}
}