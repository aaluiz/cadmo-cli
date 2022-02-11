using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Services
{
    [AddService]
    public class NamespaceHandler : INamespaceHandler
    {

        public NamespaceHandler()
        {
        }

        public ImmutableList<Type> GetClasses()
        {
         
            var types = AppDomain.CurrentDomain.GetAssemblies();
            var result = types
                .SelectMany(t => t.GetTypes())
                .ToImmutableList();
            return result;
        }

        public ImmutableList<MethodInfo> GetMethods(Type type)
        {
            var result = type.GetMethods()
				.ToImmutableList();
            return result;
        }
    }
}
