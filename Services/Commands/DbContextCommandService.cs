using Contracts.Interfaces;
using Services.Abstract;

namespace Services.Commands {

    [AddService]
	public class DbContextCommandService : AbstractService, IDbContextCommandService
	{
		private readonly ICodeGenerator _codeGenerator;
		public  DbContextCommandService(ICodeGenerator codeGenerator)
        {
			_codeGenerator = codeGenerator;
		}
        
		public int Execute(string[] args)
		{
            if (!ValidateArgs(args)) return -1;

			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("dbcontext", args);
		}
	}
}