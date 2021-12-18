using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface INamespaceHandler
    {
        ImmutableList<Type> GetClasses();
        ImmutableList<MethodInfo> GetMethods(Type type);
    }
}
