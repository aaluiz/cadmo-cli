using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface ISelectAllView<ViewGetModel>
    {
        List<ViewGetModel> SelectAllView();
    }

    public interface ISelectAllWithDependencyView<ViewGetModel>
    {
        List<ViewGetModel> SelectAllView(int RelationId);
    }
}