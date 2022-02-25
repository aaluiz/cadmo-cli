using System.Collections.Immutable;

namespace Contracts.Interfaces
{
	public interface IDirectoryHandler
	{
		bool ModelExist(string currentDirectory, string[] args);
		ImmutableList<string> GetModelNames(string currentDirectory);
	}
}