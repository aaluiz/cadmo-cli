using Contracts.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ICreateInterfaceGenerator
    {
        FileCode CreateInterface(string name, string nameSpace);

        FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods);

        FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties);

        FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties);
    }
}
