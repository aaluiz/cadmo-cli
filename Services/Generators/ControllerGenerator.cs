using Contracts.Abstractions;
using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace Services
{
    [AddService]
    public class ControllerGenerator : CommandAbrstract, IControllerGenerator
    {
        public ControllerGenerator(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder, IPathManager pathManager, ITypeProcessor typeProcessor) : base(namespaceHandler, classDefinition, methodDefinition, fileBuilder, pathManager, typeProcessor)
        {
        }

        public ImmutableList<FileCode> GetResultNull()
        {
            List<FileCode> result = new List<FileCode>();
            return result.ToImmutableList();
        }



        public override ImmutableList<FileCode> GenClasses()
        {
            if (_namespaceHandler == null) return GetResultNull();
            if (_pathManager == null) return GetResultNull();
            if (_fileBuilder == null) return GetResultNull();

            var result = GenerateCode(GetConfigurationToClasses(_namespaceHandler.GetClasses()));
            _fileBuilder.WriteFiles(result, string.IsNullOrEmpty(_pathManager.GetPath()) ? "Controllers" : _pathManager.GetPath());
            return result;
        }

        public override ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements)
        {


            var result = classElements.Select(x =>
            {
                var methodsList = x.Methods!.ToList();
                var constructor = new MethodElements
                {
                    Name = string.Format("{0}Controller", x.Name),
                    Parameters = new List<Parameter>()
                    {
                        new Parameter
                        {
                            Name = "service",
                            Type = x!.Properties!.FirstOrDefault()!.TypeProperty
                        }
                    }.ToImmutableList(),
                    LogicContent = "\t _service = service;",
                    ReturnDefinition = new ReturnDefinition
                    {
                        Type = "",
                        Visibility = Visibility.Public,
                        IsConstructor = true
                    },
                };
                var methods = new List<MethodElements>();
                methods.Add(constructor);
                methods.AddRange(methodsList);


                var classCode = _classDefinition!.Builder
                    .Imports(x!.Imports!)
                    .Inheritance(x!.Inheritance!)
                    .Properties(x.Properties!)
                    .Methods(methods.Select(m =>
                    {
                        return _methodDefiniton!.Builder
                            .Name(m.Name!)
                            .Annotations(m.Annotations!)
                            .Parameters(m.Parameters)
                            .ReturnDefinition(m.ReturnDefinition!)
                            .LogiContent(m.LogicContent!)

                            .Create();
                    }).ToImmutableList())
                    .Name(string.Format("{0}Controller", x.Name))
                    .Namespace(x.Namespace!)
                    .Create().ClassCode;
                string fileName = string.Format("{0}Controller.cs", x.Name);
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
            List<string> excluded = new List<string> { "" };
            var result = types

           .Where(s => s.BaseType!.IsGenericType)
           .Where(e => !excluded.Contains(e.Name))
           .Select(t =>
           {
               if (t.GenericTypeArguments.Length > 0)
               {
                   Type dt = t.GenericTypeArguments[0];
               }
               return new ClassElements
               {
                   Name = string.Format("{0}", t.Name),
                   Namespace = " CreanetCoreApi.Controllers",
                   Imports = new string[] {
                       "System",
                       "Contracts",
                       "DataManagerCSharp",
                       "System.Collections.Generic",
                       "System.Web.Http.Description",
                       "System.Web.Mvc",
                         "CreanetCoreApi.Controllers.Abstract",
                         "DTCREANET",
                         "DataManagerCSharp.Lookup",
                         "ConfeaAPI.Models",
                         "System.Collections",
                         "System.Net.Mail"
                   }.ToImmutableList(),
                   Inheritance = new string[] { "ApiControllerBase" }.ToImmutableList(),
                   Methods = GetConfigurationToMethods(t.GetMethods().ToImmutableList()),
                   Properties = new List<Property> {
                    new Property
                    {
                         Visibility = Visibility.None,
                         Name = "_service",
                         TypeProperty = string.Format("I{0}<{1}>", t.Name, GetGenericParameter(t))
                    }
                   }.ToImmutableList()
               };
           })
            .ToImmutableList();
            return result;
        }

        private string GetGenericParameter(Type type)
        {
            var baseType = type.BaseType!;
            string result = baseType!.GenericTypeArguments!.FirstOrDefault()!.Name;
            return result;
        }

        private bool IsDuplicatedMethod(ImmutableList<MethodInfo> methods, MethodInfo method)
        {
            int incidents = methods.Count(x => x.Name == method.Name);

            return incidents > 1;
        }

        private string GetLastParameterName(ImmutableList<MethodInfo> methods, MethodInfo method)
        {
            var selected = methods
                    .Where(x => x.Name == method.Name).ToList();

            var selectedWithLenth = selected.Select(x =>
            {
                return new Tuple<MethodInfo, int>(x, x.GetParameters().Count());
            });


            var bigger = method.GetParameters().Length - 1;
            return string.Format("{0}With{1}", method.Name, method.GetParameters()[bigger].Name);
        }

        private string GenSingleName(MethodInfo method)
        {
            string result = string.Format("{0}With{1}",
                method.Name,
                string.Join("", method.GetParameters().Select(x => x.Name).ToList()));

            return result;
        }


        private string FreeOfDuplicatedName(ImmutableList<MethodInfo> methods, MethodInfo method)
        {
            if (method.GetParameters().Count() == 0) return method.Name;

            if (IsDuplicatedMethod(methods, method))
            {
                return GenSingleName(method);
            }

            return method.Name;
        }


        public string GetOutOrRef(ParameterInfo parameter)
        {
            if (parameter.ParameterType.IsByRef && parameter.IsOut) return "out";
            if (parameter.ParameterType.IsByRef && parameter.IsOut == false) return "ref";
            return "";
        }

        private bool isOut(ParameterInfo parameter)
        {
            return (parameter.ParameterType.IsByRef && parameter.IsOut);
        }

        public override ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods)
        {
            var nullReturn = new List<MethodElements>();
            if (_typeProcessor == null) return nullReturn.ToImmutableList();

            var basicMethods = new string[] {
                "Dispose",
                "Equals",
                "GetHashCode",
                "GetType",
                "ToString",
                "CallServer",
                "LookupFilterColumns",
                "LookupGridColumns",
                "FindByCode",
                "Find",
                "CheckIndicator"
            }.ToImmutableList();

            var result = methods
                .Where(f => (!basicMethods.Contains(f.Name)))
                .Where(g => !g.Name.Contains("get_"))
                .Where(s => !s.Name.Contains("set_"))
                .Where(s => !s.Name.Contains("add_"))
                .Where(s => !s.Name.Contains("remove_"))
                .Where(c => !c.IsConstructor)
                .Where(a => !a.IsStatic)
                .Select((m) =>
                {
                    return new MethodElements
                    {
                        Annotations = ProcessAnnotations(FreeOfDuplicatedName(methods, m), _typeProcessor.ReflectionToCode(m.ReturnType, true, true)),
                        Name = m.IsGenericMethod ? string.Format("{0}<T>", m.Name) : FreeOfDuplicatedName(methods, m),
                        Parameters = m.GetParameters()
                          .Select((p) =>
                          {
                              return new Parameter
                              {
                                  Name = p.Name,
                                  Type = _typeProcessor.ReflectionToCode(p.ParameterType, true, false, GetOutOrRef(p)).Trim()
                              };
                          }).ToImmutableList(),
                        LogicContent = ProcessLogicContent(m.GetParameters(), m),
                        isInterface = false,
                        ReturnDefinition = new ReturnDefinition
                        {
                            Type = "System.Web.Http.IHttpActionResult",
                            Visibility = Visibility.Public
                        }
                    };
                }).ToImmutableList();

            return result;
        }

        private ImmutableList<string> ProcessAnnotations(string MethodName, string ReturnType)
        {
            string firstAnnotation = string.Format("Route(\"{0}\")", MethodName);
            string secondAnnotation = string.Format("ActionName(\"{0}\")", MethodName);
            if (ReturnType.Contains("void"))
            {
                return new string[] { firstAnnotation, secondAnnotation }.ToImmutableList();
            }

            string responseType = string.Format("ResponseType(typeof({0}))", ReturnType);

            var result = new string[] { firstAnnotation, secondAnnotation, responseType }.ToImmutableList();
            return result;
        }

        private string SetOutVariable(MethodInfo method)
        {
            var parameters = method.GetParameters().Where(x => isOut(x)).ToList();

            if (parameters.Count > 0)
            {
                return string.Join("\n", parameters.Select(x =>
                {
                    if (x.ParameterType == typeof(bool))
                    {
                        return string.Format("{0} = false;", x.Name);
                    }
                    return string.Format("{0} = null;", x.Name);
                }).ToList());
            }

            string result = "";
            return result;
        }

        private string ProcessLogicContent(ParameterInfo[] parameters, MethodInfo method)
        {
            string result = "";
            StringBuilder code = new StringBuilder();
            string onlyNames = string.Join(",", parameters.Select(x => string.Format("{0} {1}", GetOutOrRef(x), x.Name)).ToArray());
            if (parameters.Count() > 0)
            {
                if (method.ReturnType == typeof(void))
                {
                    code.AppendLine("\ttry");
                    code.AppendLine("\t\t{");
                    code.AppendLine(string.Format("\t\t\t_service.{0}({1});", method.Name, onlyNames));
                    code.AppendLine("\t\t}");
                    code.AppendLine("\t\t\tcatch (MessageFromBaseException e)");
                    code.AppendLine("\t\t{");
                    code.AppendLine(SetOutVariable(method));
                    code.AppendLine("\t\t\treturn ProcessResponse(ShowErrorMessage(e.Message));");
                    code.AppendLine("\t\t}");
                    code.AppendLine("\t\treturn ProcessResponse(\"Processado com sucesso!!\");"); ;

                    result = code.ToString();
                    return result;
                }
                result = string.Format("\t\t var result = _service.{0}({1}); \n\t return ProcessResponse(result); ", method.Name, onlyNames);
                return result;
            }

            if (method.ReturnType == typeof(void))
            {

                code.AppendLine("\ttry");
                code.AppendLine("\t\t{");
                code.AppendLine(string.Format("\t\t\t_service.{0}();", method.Name));
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t\t catch (MessageFromBaseException e)");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\treturn ProcessResponse(ShowErrorMessage(e.Message));");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\treturn ProcessResponse(\"Processado com sucesso!!\");"); ;

                result = code.ToString();
                return result;
            }

            result = string.Format("\t var result = _service.{0}(); \n\t return ProcessResponse(result); ", method.Name);
            return result;
        }
    }
}
