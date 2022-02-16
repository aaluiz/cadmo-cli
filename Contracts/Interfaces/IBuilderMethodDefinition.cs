using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    /// <summary>
    /// Generate blocks of codes
    /// </summary>
    public interface IBuilderMethodDefinition
    {
        IBuilderMethodDefinition Name(string name);
        IBuilderMethodDefinition Parameters(ImmutableList<Parameter>? parameters = null);
        IBuilderMethodDefinition Annotations(ImmutableList<string> annotations);
		IBuilderMethodDefinition BaseImplementationObjects(ImmutableList<string>? baseObjects);
        IBuilderMethodDefinition ReturnDefinition(ReturnDefinition returnDefinitions);
        IBuilderMethodDefinition LogiContent(string logicContent);
        IMethodDefinition Create();
        IMethodDefinition InterfaceCreate();
	}
}
