using System.Runtime.InteropServices;
using Contracts.Interfaces;


public class AbstractGeneratorService
{
	protected IClassDefinition? _classDefinition;
	protected IMethodDefinition? _methodDefinition;
	protected IFileBuilder? _fileBuilder;
	private IClassDefinition? classDefinition;
	private IMethodDefinition? methodDefinition;

	public AbstractGeneratorService(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder)
	{
		_classDefinition = classDefinition;
		_methodDefinition = methodDefinition;
		_fileBuilder = fileBuilder;
	}

	protected string CurrentDirectory
	{
		get
		{
			return Environment.CurrentDirectory;
		}
	}

	protected bool IsDefaultPath(string path)
	{
		return !(path.Contains("\\") || path.Contains("/"));
	}

	protected static bool isWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	protected static bool isMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	protected static bool isLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
}