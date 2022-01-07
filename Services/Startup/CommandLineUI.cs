
using Contracts.Interfaces;



namespace Services.Startup
{
	[AddService]
	public class CommandLineUI : ICommandLineUI
	{
		public int ExecuteCommmand(string[] args)
		{
			Console.WriteLine("oi generators");
			return 1;
		}
	}
}