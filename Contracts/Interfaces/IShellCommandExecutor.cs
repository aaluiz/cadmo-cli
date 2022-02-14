namespace Contracts.Interfaces
{
    public interface IShellCommandExecutor
    {
        bool ExecuteCommand(string command,
            string args);

        bool ExecuteCommand(string command, string args, string executionPath);
    }
}