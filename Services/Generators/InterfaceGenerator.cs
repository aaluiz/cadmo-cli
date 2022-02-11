using Contracts.Abstractions;
using Contracts.Interfaces;
using System.Collections.Immutable;
using Models;
using System.Reflection;

namespace Services
{
    [AddService]
    public class InterfaceGenerator : CommandAbrstract, IInterfaceGenerator
    {
        public InterfaceGenerator(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder, IPathManager pathManager, ITypeProcessor typeProcessor) : base(namespaceHandler, classDefinition, methodDefinition, fileBuilder, pathManager, typeProcessor)
        {
        }

        public override ImmutableList<FileCode> GenClasses()
        {
            var result = GenerateCode(GetConfigurationToClasses(_namespaceHandler!.GetClasses()));
            _fileBuilder!.WriteFiles(result, string.IsNullOrEmpty(_pathManager!.GetPath()) ? "Contracts" : _pathManager.GetPath());
            return result;
        }

        public override ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements)
        {
            var result = classElements.Select(x =>
            {
                var classCode = _classDefinition!.Builder
                    .Imports(x.Imports!)
                    .Inheritance(x.Inheritance!)
                    .Methods(x.Methods!.Select(m =>
                    {
                        return _methodDefiniton!.Builder
                            .Name(m.Name!)
                            .Parameters(m.Parameters)
                            .ReturnDefinition(m.ReturnDefinition!)
                            .LogiContent(m.LogicContent!)
                            .InterfaceCreate();
                    }).ToImmutableList())
                    .Name(x.Name!, true)
                    .Namespace(x.Namespace!)
                    .Create().ClassCode;
                string fileName = string.Format("{0}.cs", x.Name!.Replace("<DataTable>", ""));
                return new FileCode
                {
                    FileName = fileName,
                    Code = classCode
                };
            }).ToImmutableList();
            return result;
        }

        private void SetDataTableType(Type type)
        {
            if (type.BaseType!.IsGenericType)
            {
                _typeProcessor!.MainDataTable = type.BaseType.GenericTypeArguments[0].Name;
            }
            else
            {
                _typeProcessor!.MainDataTable = "";
            }
        }

        private bool BlockedList(string name)
        {
            var list = new string[] { "" }.ToImmutableList();

            bool result = list.Contains(name);

            return result;
        }

        public override ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types)
        {
            var result = types
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsSealed)
                .Where(x => !BlockedList(x.Name))
                .Select(t =>
                {
                    SetDataTableType(t);
                    return new ClassElements
                    {
                        Name = string.Format("I{0}<DataTable>", t.Name),
                        Namespace = "Contracts",
                        Imports = new string[] {
                        "System",
                        "System.Data",
                        "System.Text",
                        "System.Collections.Generic",
                        "DataManagerCSharp",
                        "DataManagerCSharp.Interfaces",
                        "BMCREANET",
                        "static BMCursoProf",
                        "System.Net.Mail",
                         "Microsoft.Reporting.WebForms",
                         "static BMCertPeticao",
                         "DataManagerCSharp.Lookup",
                         "System.Collections",
                         "ConfeaAPI.Models",
                }.ToImmutableList(),
                        Inheritance = new string[] { "IBusinessServer<DataTable>" }.ToImmutableList(),
                        Methods = GetConfigurationToMethods(t.GetMethods().ToImmutableList()),

                    };
                })
                .ToImmutableList();
            return result;
        }


        public string GetOutOrRef(ParameterInfo parameter)
        {
            if (parameter.ParameterType.IsByRef && parameter.IsOut) return "out";
            if (parameter.ParameterType.IsByRef && parameter.IsOut == false) return "ref";
            return "";
        }

        public override ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods)
        {
            var basicMethods = new string[] {
                "Dispose",
                "Equals",
                "GetHashCode",
                "GetType",
                "ToString",
                "CallServer",
            }.ToImmutableList();

            var result = methods
                .Where(f => (!basicMethods.Contains(f.Name)))
                .Where(g => !g.Name.Contains("get_"))
                .Where(s => !s.Name.Contains("set_"))
                .Where(s => !s.Name.Contains("add_"))
                .Where(s => !s.Name.Contains("remove_"))
                //.Where(z => !z.IsVirtual)
                .Where(c => !c.IsConstructor)
                .Where(a => !a.IsStatic)
                .Select((m) =>
                {
                    //if (m.Name.Contains("ConverteEnderecos")){
                    //    Console.WriteLine("ConverteEnderecos");
                    //} 
                    return new MethodElements
                    {
                        Name = m.IsGenericMethod ? string.Format("{0}<T>", m.Name) : m.Name,
                        Parameters = m.GetParameters()
                          .Select((p) =>
                          {
                              return new Parameter
                              {
                                  Name = p.Name,
                                  Type = _typeProcessor!.ReflectionToCode(p.ParameterType, true, false, GetOutOrRef(p))
                              };
                          }).ToImmutableList(),
                        isInterface = true,
                        ReturnDefinition = new ReturnDefinition
                        {
                            Type = _typeProcessor!.ReflectionToCode(m.ReturnType, true),
                            Visibility = Visibility.None
                        }
                    };
                }).ToImmutableList();

            return result;
        }
    }
}
