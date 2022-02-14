using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;
namespace Services
{
    [AddService]
    public class CreateInterfaceGenerator : AbstractGeneratorService, ICreateInterfaceGenerator
    {
        public CreateInterfaceGenerator(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder) : base(classDefinition, methodDefinition, fileBuilder)
        {
        }

        public FileCode CreateInterface(string name, string nameSpace)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, true)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }


        public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, true)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, true)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, true)
                .Methods(methods)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }
        private static FileCode GetFileCode(string name, IClassDefinition builder)
        {
            return new FileCode { Code = builder.ClassCode, FileName = $"{name}.cs" };
        }
    }
}
