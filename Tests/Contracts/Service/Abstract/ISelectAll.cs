using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface ISelectAll<Model>
    {
        List<Model> SelectAll();
    }

    public interface ISelectAllWithDependency<Model>
    {
        List<Model> SelectAll(int RelationId);
    }
}