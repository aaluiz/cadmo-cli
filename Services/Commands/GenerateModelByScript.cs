using Services.Abstract;
using Contracts.Interfaces;

namespace Services.Commands
{

	[AddService]
	public class GenerateModelByScript : AbstractService, IGenerateModelByScript
	{
		private readonly ICodeGenerator _codeGenerator;
		private readonly IMethodDefinition _methodDefinition;

		public GenerateModelByScript(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			switch (args[2])
			{
				case "--with-script": return GenerateModel(args[3]);
				case "--with-all-scripts":
					switch (args[3])
					{
						case "--safety": return SafetyGenerate();
						case "--force": return ForceGenerate();
						default:
							return -1;
					}
				default:
					return -1;
			}
		}

		private int ForceGenerate()
		{

			return 1;
		}

		private int SafetyGenerate()
		{

			return 1;
		}

		private int GenerateModel(string scriptModelName)
		{

			return 1;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("model", args);
		}
	}
}