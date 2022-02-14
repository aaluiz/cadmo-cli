using Contracts.Abstractions;
using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;
using System.Reflection;

namespace Services
{
    [AddService]
    public class TestServiceGenerator : CommandAbrstract, ITestServiceGenerator
    {
        public TestServiceGenerator(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder, IPathManager pathManager, ITypeProcessor typeProcessor) : base(namespaceHandler, classDefinition, methodDefinition, fileBuilder, pathManager, typeProcessor)
        {
        }

        public override ImmutableList<FileCode> GenClasses()
        {
            var result = new List<FileCode>();
            return result.ToImmutableList();
        }

        public override ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements)
        {
            throw new NotImplementedException();
        }

        public override ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types)
        {
            throw new NotImplementedException();
        }

        public override ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods)
        {
            throw new NotImplementedException();
        }

        private bool Process()
        {
            bool result = false;
            var classes = _namespaceHandler!.GetClasses();


            return result;
        }
    }
}
