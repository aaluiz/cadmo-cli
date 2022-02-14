namespace Contracts.Interfaces
{
    public interface ICommand
    {
        int Execute(string[] args);
    }
}