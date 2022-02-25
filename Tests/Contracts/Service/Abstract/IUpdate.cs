namespace Contracts.Service.Abstract
{
    public interface IUpdate<ViewModel>
    {
        bool Update(ViewModel ViewModel);
    }
}