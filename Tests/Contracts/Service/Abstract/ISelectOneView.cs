namespace Contracts.Service.Abstract
{
    public interface ISelectOneView<ViewGetModel>
    {
        ViewGetModel SelectOneViewById(int Id);
    }
}