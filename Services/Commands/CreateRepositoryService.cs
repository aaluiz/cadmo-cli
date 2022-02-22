using Services.Abstract;
using Contracts.Interfaces;

namespace Services.Commands
{
    [AddService]
	public class CreateRepositoryService : AbstractService, ICreateRepositoryService
	{
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("repository", args);
		}
	}
}