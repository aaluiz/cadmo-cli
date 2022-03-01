using Contracts.Interfaces;

namespace Services.Generators
{
	[AddService]
	public class CodeGenerator : ICodeGenerator
	{
		public ICreateClassGenerator ClassGenerator { get; }

		public ICreateInterfaceGenerator InterfaceGenerator { get; }

		public IFileBuilder FileBuilder { get; }

		public IBuilderClassDefinition BuilderCustonClass { get; }

		public CodeGenerator(ICreateClassGenerator createClassGenerator, ICreateInterfaceGenerator interfaceGenerator, IFileBuilder fileBuilder, IBuilderClassDefinition builderClassDefinition)
        {
			this.ClassGenerator = createClassGenerator;
			this.InterfaceGenerator = interfaceGenerator;
			this.FileBuilder = fileBuilder;
			this.BuilderCustonClass = builderClassDefinition;
		}
	}
}