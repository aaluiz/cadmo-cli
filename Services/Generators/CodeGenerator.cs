using Contracts.Interfaces;

namespace Services.Generators
{
	[AddService]
	public class CodeGenerator : ICodeGenerator
	{
		public ICreateClassGenerator ClassGenerator { get; }

		public ICreateInterfaceGenerator InterfaceGenerator { get; }

		public IFileBuilder FileBuilder { get; }

	public CodeGenerator(ICreateClassGenerator createClassGenerator, ICreateInterfaceGenerator interfaceGenerator, IFileBuilder fileBuilder)
        {
			this.ClassGenerator = createClassGenerator;
			this.InterfaceGenerator = interfaceGenerator;
			this.FileBuilder = fileBuilder;
		}
	}
}