using Models;
namespace Contracts.Interfaces
{
	public interface IAutoMapperCommandService: ICommand
    {
		FileCode GetFileCodeAutoMapper();
	}
}