using Contracts.Abstractions;
using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;
using System.Reflection;

namespace Services
{
    [AddService]
    public class ServiceGenerator : CommandAbrstract, IServiceGenerator
    {
        public ServiceGenerator(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder, IPathManager pathManager, ITypeProcessor typeProcessor) : base(namespaceHandler, classDefinition, methodDefinition, fileBuilder, pathManager, typeProcessor)
        {
        }

        public override ImmutableList<FileCode> GenClasses()
        {
            var result = GenerateCode(GetConfigurationToClasses(_namespaceHandler!.GetClasses()));
            _fileBuilder!.WriteFiles(result, string.IsNullOrEmpty(_pathManager!.GetPath()) ? "Services" : _pathManager.GetPath());
            return result;
        }

        public override ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements)
        {
            var result = classElements.Select(x =>
            {
                var classCode = _classDefinition!.Builder
                    .Imports(x.Imports!)
                    .Inheritance(x.Inheritance!)
                    .Name(x.Name!)
                    .Namespace(x.Namespace!)
                    .Create().ClassCode;
                string fileName = string.Format("{0}.cs", x.Name);
                return new FileCode
                {
                    FileName = fileName,
                    Code = classCode
                };
            }).ToImmutableList();
            return result;
        }

        public override ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types)
        {

            Func<Type, string> GetDT = (x =>
            {
                return x.BaseType!.GenericTypeArguments[0].Name;
            });
            var result = types
                .Where(s => s.BaseType!.IsGenericType)
                .Select(t => new ClassElements
                {
                    Name = t.Name,
                    Namespace = "",
                    Imports = new string[] {
                    }.ToImmutableList(),
                    Inheritance = new string[] { t.Name, string.Format("I{0}<{1}>", t.Name, GetDT(t)) }.ToImmutableList(),
                }).ToImmutableList();
            return result;
        }

        public override ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods)
        {
			var result = new List<MethodElements>();
			return result.ToImmutableList();
        }
    }
}

