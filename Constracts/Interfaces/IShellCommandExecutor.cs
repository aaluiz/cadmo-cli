namespace Contracts.Interfaces
{
	public interface IShellCommandExecutor
	{
		bool ExecuteCommand(string command,
			string args);
	}
}