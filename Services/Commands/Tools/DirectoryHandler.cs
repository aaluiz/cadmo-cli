using System.Collections.Immutable;
using System.Reflection;
using Contracts.Interfaces;

namespace Services.Commands.Tools
{

	[AddService]
	public class DirectoryHandler : IDirectoryHandler
	{
		public ImmutableList<string> GetModelNames(string currentDirectory)
		{
			return Directory
				.GetFiles($"{currentDirectory}/Entities/Models/")
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToImmutableList();
		}

		public bool ModelExist(string currentDirectory, string[] args)
		{
			var models = Directory
				.GetFiles($"{currentDirectory}/Entities/Models/")
					.Select(x => Path.GetFileNameWithoutExtension(x))
						.ToList();

			bool result = models.Contains(args[2]);

			if (!result) System.Console.WriteLine("Model doesn't exist. Create new Model or check the spelling, and try again.");

			return result;
		}

		public string GetFileFromAsset(string fileName)
		{
			string codeBase = Assembly.GetExecutingAssembly().Location;
			string assemblyPath = Path.GetDirectoryName(codeBase)!;
			return $"{assemblyPath}/Assets/{fileName}";
		}

		public ImmutableList<string> GetServiceNames(string currentDirectory)
		{
			return Directory
				.GetFiles($"{currentDirectory}/Services/")
				.Where(y => !y.Contains("csproj"))
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToImmutableList();
		}

		public void DeleteClass1File(string currentDirectory, string path)
		{
			File.Delete($"{currentDirectory}/{path}/Class1.cs");
		}
	}
}