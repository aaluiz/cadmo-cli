namespace Contratos.Service.Abstract
{
    public interface IInsert<ViewModel>
    {
        bool NewRecord(ViewModel ViewModel);
    }
}