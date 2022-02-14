using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [AddService]
    public class CreateClassGenerator : AbstractGeneratorService, ICreateClassGenerator
    {
        public CreateClassGenerator(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder) : base(classDefinition, methodDefinition, fileBuilder)
        {
        }

        public FileCode CreateClass(string name, string nameSpace)
        {
            throw new NotImplementedException();
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods)
        {
            throw new NotImplementedException();
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> methods)
        {
            throw new NotImplementedException();
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, IImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties)
        {
            throw new NotImplementedException();
        }
    }
}
