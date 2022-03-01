
using System.Collections.Immutable;

namespace Services.Abstract
{
    public abstract class AbstractService
    {
        protected static string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        protected bool IsValidArgs(string[] args)
        {
            return (args.Length <= 5);
        }

        protected bool IsTheReservedWord(string word, string[] args)
        {
            if (args[0] == "g" || args[0] == "generate" || args[0] == "update") return (args[1] == word);
            return (args[0] == word);
        }
        protected static bool IsDefaultPath(string path)
        {
            return !(path.Contains('\\') || path.Contains('/'));
        }

        
		protected bool ModelExist(string[] args){
			var models = Directory
				.GetFiles($"{CurrentDirectory}/Entities/Models/")
				    .Select(x => Path.GetFileNameWithoutExtension(x))
				        .ToList();

			bool result = models.Contains(args[2]);

			if (!result) System.Console.WriteLine("Model doesn't exist. Create new Model or check the spelling, and try again.");

			return result;
		}

        protected ImmutableList<string> GetModelNames(){
            return Directory
				.GetFiles($"{CurrentDirectory}/Entities/Models/")
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToImmutableList();
        }

        protected abstract bool ValidateArgs(string[] args);

    }
}