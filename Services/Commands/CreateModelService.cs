using Contracts.Interfaces;


[AddService]
public class CreateModelService : AbstractGeneratorService, ICreateModelService
{
	public CreateModelService(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder) : base(classDefinition, methodDefinition, fileBuilder)
	{
	}

	public int Execute(string[] args)
	{
		return 1;
	}
}