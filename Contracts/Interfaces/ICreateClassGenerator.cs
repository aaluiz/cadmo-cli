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
    public interface ICreateClassGenerator
    {
        FileCode CreateClass(string name, string nameSpace);

        FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods);

        FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> methods);

        FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, IImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties);
    }
}
