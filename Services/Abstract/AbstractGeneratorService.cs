using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Contracts.Interfaces;
using Models;

namespace Services.Abstract
{

    public abstract class AbstractGeneratorService
    {
        protected IClassDefinition? _classDefinition;
        protected IMethodDefinition? _methodDefinition;

        public AbstractGeneratorService(IClassDefinition classDefinition, IMethodDefinition methodDefinition)
        {
            _classDefinition = classDefinition;
            _methodDefinition = methodDefinition;
        }
    }
}