using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;

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
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods)
        {

            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
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
