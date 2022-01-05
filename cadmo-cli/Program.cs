﻿using Contracts.Interfaces;
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
					commandLineUI.ExecuteCommmand(args);
				}
				return host.StartAsync();
			}
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
					 .ConfigureServices((_, services) =>
								services.AddServicesToDI()
					 );
	}
}