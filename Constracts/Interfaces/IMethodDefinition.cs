using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IMethodDefinition
    {
        string MethodCode { get; }

        IBuilderMethodDefinition Builder { get; set; }
    }
}
