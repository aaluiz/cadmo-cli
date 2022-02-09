using System.Diagnostics;
using Contracts.Interfaces;
using Microsoft.Extensions.Hosting;
using Services;

namespace Services
{
	[AddService]	
	public class ShellCommandExecutor: IShellCommandExecutor {
		public bool ExecuteCommand(string command, string args)
		{
			Process process = new Process(){
				StartInfo =  CreateStartInfo(command, args)
			};
			return process.Start();
		}

		private ProcessStartInfo CreateStartInfo(string command, string args){
			ProcessStartInfo startInfo = new ProcessStartInfo(){
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
				FileName = command,
				Arguments = args
			};
			return startInfo;
		}

	}
		
}