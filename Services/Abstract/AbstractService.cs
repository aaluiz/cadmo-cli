
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
            return (args.Length == 1|| args.Length == 2 || args.Length == 3);
        }

        protected bool IsTheReservedWord(string word, string[] args)
        {
            if (args[0] == "g" || args[0] == "generate") return (args[1] == word);
            return (args[0] == word);
        }
        protected static bool IsDefaultPath(string path)
        {
            return !(path.Contains('\\') || path.Contains('/'));
        }

        protected abstract bool ValidateArgs(string[] args);

    }
}