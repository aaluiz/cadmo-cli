using Contracts.Interfaces;
using Models;


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

	private FileCode
}