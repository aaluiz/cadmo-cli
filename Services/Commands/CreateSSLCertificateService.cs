using Contracts.Interfaces;
using Services.Abstract;

namespace Services
{


	[AddService]
	public class CreateSSLCertificateService : AbstractService, ICreateSSLCertificateService
	{
		IShellCommandExecutor _shellCommandExecutor;
		public CreateSSLCertificateService(IShellCommandExecutor shellCommandExecutor)
        {
			_shellCommandExecutor = shellCommandExecutor;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			_shellCommandExecutor.ExecuteCommand("dotnet", "dev-certs https --clean");
			_shellCommandExecutor.ExecuteCommand("dotnet", "dev-certs https --trust");
			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReserverWord("ssl", args);
		}

	}
}