using Contracts.Interfaces;

[AddService]
public class HelpService : IHelpService
{
    public int Execute(string[] args)
    {
        System.Console.WriteLine(Help());
        return 1;
    }


    private string Help()
    {
        return @"cadmo-cli new <AppName>
			
cadmo-cli [-g | generate] model <ModelName>
			
cadmo-cli [-g | generate] repository <RepositoryName> 
			
cadmo-cli [-g | generate] service <ServiceName>
			
cadmo-cli [-g | generate] controler <ControllerName>
			
cadmo-cli [-g | generate] endpoint <EndpointName> ";
    }
}