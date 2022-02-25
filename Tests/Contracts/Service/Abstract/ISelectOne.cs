namespace Contracts.Service.Abstract
{
    public interface ISelectOne<Model>
    {
        Model SelectById(int Id);
    }
}