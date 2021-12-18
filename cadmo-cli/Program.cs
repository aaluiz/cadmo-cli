using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
                   // ICommandLineUI commandLineUI = provider.GetRequiredService<ICommandLineUI>();
                   // commandLineUI.ExecuteCommmand(args);
                }
                return host.StartAsync();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) => 
            //Host.CreateDefaultBuilder(args)
              //  .ConfigureServices((_, services) =>
                    // services.addtransient<icommandlineui, commandlineui>()
                    //     .addtransient<icontrollergenerator, controllergenerator>()
                    //     .addtransient<iservicegenerator, servicegenerator>()
                    //     .addtransient<iinterfacegenerator, interfacegenerator>()
                    //     .addtransient<itestservicegenerator, testservicegenerator>()
                    //     .addtransient<iiocgenerator, iocconfiggenerator>()
                    //     .addtransient<imethoddefinition, methoddefiner>()
                    //     .addtransient<ibuildermethoddefinition, buildermethoddefinition>()
                    //     .addtransient<iclassdefinition, classdefiner>()
                    //     .addtransient<ibuilderclassdefinition, builderclassdefinition>()
                    //     .addtransient<inamespacehandler, namespacehandler>()
                    //     .addtransient<ifilebuilder, filebuilder>()
                    //     .addsingleton<ipathmanager, pathmanager>()
                        // .addsingleton<itypeprocessor, typeprocessor>()
               // );
    }
}