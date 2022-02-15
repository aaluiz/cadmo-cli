using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;
namespace Services
{
    [AddService]
    public class CreateInterfaceGenerator : AbstractGeneratorService, ICreateInterfaceGenerator
    {
        public CreateInterfaceGenerator(IClassDefinition classDefinition, IMethodDefinition methodDefinition) : base(classDefinition, methodDefinition)
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
                .Imports(imports)
                .Name(name, true)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
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
                .Imports(imports)
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

		public FileCode CreateInterface(string name, string nameSpace, ImmutableList<string> inheritance)
		{
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, true)
                .Inheritance(inheritance)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
		}

		public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<string> inheritance)
		{
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name, true)
                .Inheritance(inheritance)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
		}
		public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties, ImmutableList<string> inheritance)
		{
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name, true)
                .Inheritance(inheritance)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
		}

		public FileCode CreateInterface(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties, ImmutableList<string> inheritance)
		{
            var builder = _classDefinition!.Builder
                .Imports(imports)
                .Namespace(nameSpace)
                .Name(name, true)
                .Methods(methods)
                .Inheritance(inheritance)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
		}
	}
}
