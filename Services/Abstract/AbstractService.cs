
namespace Services.Abstract
{
    public abstract class AbstractService
    {

        protected bool IsValidArgs(string[] args)
        {
            return (args.Length == 2 || args.Length == 3);
        }

        protected bool IsTheReserverWord(string word, string[] args)
        {
            if (args[0] == "-g" || args[0] == "generate") return (args[1] == word);
            return (args[0] == word);
        }

        protected abstract bool ValidateArgs(string[] args);

    }
}