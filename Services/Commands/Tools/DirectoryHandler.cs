using System.Collections.Immutable;
using Contracts.Interfaces;

namespace Services.Commands.Tools
{

    [AddService]
	public class DirectoryHandler : IDirectoryHandler
	{
		public ImmutableList<string> GetModelNames(string currentDirectory){
            return Directory
				.GetFiles($"{currentDirectory}/Entities/Models/")
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToImmutableList();
		}

		public bool ModelExist(string currentDirectory ,string[] args)
		{
			var models = Directory
				.GetFiles($"{currentDirectory}/Entities/Models/")
				    .Select(x => Path.GetFileNameWithoutExtension(x))
				        .ToList();

			bool result = models.Contains(args[2]);

			if (!result) System.Console.WriteLine("Model doesn't exist. Create new Model or check the spelling, and try again.");

			return result;
		}
	}
}