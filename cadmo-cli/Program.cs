using System;
using System.Threading.Tasks;
using Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;


namespace corecli
{
	class Program
	{
		static Task Main(string[] args)
		{
			using (IHost host = CreateHostBuilder(args).Build())
			{
				using (IServiceScope serviceScope = host.Services.CreateScope())
				{
					IServiceProvider provider = serviceScope.ServiceProvider;
					ICommandLineUI commandLineUI = provider.GetRequiredService<ICommandLineUI>();
				    var result = commandLineUI.ExecuteCommmand(args);
					if (result ==  -1) System.Console.WriteLine(Commands());
				}
				return host.StartAsync();
			}
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
					 .ConfigureServices((_, services) =>
								services.AddServicesToDI()
					 );

		static string Commands(){
			return @"command not found, try help";
		}

	}
}