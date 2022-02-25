using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface IControlError
    {
        bool HasErro { get; }

        List<string> ErroMessage { get; }
    }
}