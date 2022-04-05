using System.Collections.Immutable;

namespace Contracts.Interfaces
{
	public interface IDirectoryHandler
	{
		bool ModelExist(string currentDirectory, string[] args);
		ImmutableList<string> GetModelNames(string currentDirectory);
		ImmutableList<string> GetServiceNames(string currentDirectory);
		ImmutableList<string> GetRespoistoryNames(string currentDirectory);

		string GetFileFromAsset(string fileName);

		void DeleteClass1File(string currentDirectory, string path);
	}
}