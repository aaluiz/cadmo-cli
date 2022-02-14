using System.Diagnostics;
using Contracts.Interfaces;
using Microsoft.Extensions.Hosting;
using Services;

namespace Services
{
    [AddService]
    public class ShellCommandExecutor : IShellCommandExecutor
    {
        public bool ExecuteCommand(string command, string args)
        {
            return CommandStarted(command, args);
        }
        public bool ExecuteCommand(string command, string args, string executionPath)
        {
            return CommandStarted(command, args, executionPath);
        }

        private bool CommandStarted(string command, string args, string? directory = null)
        {
            Process process = new Process()
            {
                StartInfo = CreateStartInfo(command, args, directory)
            };
            return process.Start();
        }


        private ProcessStartInfo CreateStartInfo(string command, string args, string? directory = null)
        {
            return new ProcessStartInfo()
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = command,
                Arguments = args,
                WorkingDirectory = directory
            };
        }
    }

}