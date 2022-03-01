namespace Contracts.Interfaces
{
    public interface ICodeGenerator
    {
		public IFileBuilder FileBuilder { get; }
		ICreateClassGenerator ClassGenerator { get; }
        ICreateInterfaceGenerator InterfaceGenerator { get; }

		IBuilderClassDefinition BuilderCustonClass { get; }
	}
}