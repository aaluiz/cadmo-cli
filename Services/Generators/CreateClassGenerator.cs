using Contracts.Interfaces;
using Models;
using Services.Abstract;
using System.Collections.Immutable;

namespace Services
{
    [AddService]
    public class CreateClassGenerator : AbstractGeneratorService, ICreateClassGenerator
    {
		public bool IsInterface { get; set; } = false;
        
		public bool IsStatic { get; set; }

		public CreateClassGenerator(IClassDefinition classDefinition, IMethodDefinition methodDefinition ) : base(classDefinition, methodDefinition)
        {
		}

        public FileCode CreateClass(string name, string nameSpace)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
			var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name, IsInterface)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }
        public FileCode CreateClass(string name, string nameSpace, ImmutableList<string> inheritance)
        {
            
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Inheritance(inheritance)
                .Name(name, IsInterface)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

		public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<string> inheritance)
		{
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name, IsInterface)
                .Inheritance(inheritance)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
		}

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods)
        {

			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Methods(methods)
                .Name(name, IsInterface)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<string> inheritance)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Inheritance(inheritance)
                .Methods(methods)
                .Name(name, IsInterface)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name, IsInterface)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }
        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties, ImmutableList<string> inheritance)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name, IsInterface)
                .Inheritance(inheritance)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }

        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Imports(imports)
                .Name(name)
                .Methods(methods)
                .Properties(properties)
                .Create();
            FileCode result = GetFileCode(name, builder);

            return result;
        }
        public FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties, ImmutableList<string> inheritance)
        {
			_classDefinition!.Builder.IsStatic = this.IsStatic;
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
                .Imports(imports)
                .Inheritance(inheritance)
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
