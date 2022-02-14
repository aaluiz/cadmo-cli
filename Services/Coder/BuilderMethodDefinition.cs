using Contracts.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Coder
{
    [AddService]
    public class BuilderMethodDefinition : IBuilderMethodDefinition
    {
        private string? _Name;
        private string? _LogigContent;
        private string? _Parameters;
        private string? _ReturnDefinition;
        private string? _Annotations;
        private bool _isConstructor;
        public IBuilderMethodDefinition Annotations(ImmutableList<string> annotations)
        {
            if (annotations != null) _Annotations = "\n\t" + string.Join("\n\t", annotations.Select(x => string.Format("[{0}]", x).TrimStart()).ToArray());
            return this;
        }

        private void IsNullOrEmptyCriticalFields()
        {
            if (string.IsNullOrEmpty(_Name)) throw new Exception("Method needs name");
            if (string.IsNullOrEmpty(_ReturnDefinition)) ReturnDefinition();
        }

        private IMethodDefinition CreateMethod(string code)
        {
            _Parameters = "";
            _LogigContent = "";
            _Name = "";
            _ReturnDefinition = "";
            _Annotations = "";
            IMethodDefinition methodDefinition = new MethodDefinition(this, code)
            {
                Builder = this
            };

            return methodDefinition;
        }

        public IMethodDefinition Create()
        {
            IsNullOrEmptyCriticalFields();
            string code = !string.IsNullOrEmpty(_Annotations) ? GenCodeWithAnnotation() : GenCode();

            return CreateMethod(code);
        }

        private string GenCode()
        {
            Func<string?, string> isProcedure = (x => string.IsNullOrEmpty(x) ? "()" : string.Format("({0})", x));
            Func<string, string> isNull = (x => string.IsNullOrEmpty(x) ? "" : x);
            if (_ReturnDefinition != "public void")
            {
                if (_ReturnDefinition != "public static void")
                {
                    if (!_isConstructor)
                    {
                        if (_LogigContent == null) throw new Exception(string.Format("Methods needs return {0} type", _ReturnDefinition));
                        if (!_LogigContent.Contains("return")) throw new Exception(string.Format("Methods needs return {0} type", _ReturnDefinition));
                    }
                }
            }

            string sessionCodeBegin = string.Format("\t{0} {1}{2}", _ReturnDefinition, _Name, isProcedure(_Parameters));

            string sessionCodeEnd = "        }";

            string resultFaseOne = sessionCodeBegin + "\n        {\n        " + _LogigContent + "\n" + sessionCodeEnd;

            string result = resultFaseOne.Replace("\r", "");


            return result;
        }

        private string GenCodeWithAnnotation()
        {
            return _Annotations + "\n" + GenCode();
        }

        public IBuilderMethodDefinition LogiContent(string logicContent)
        {
            _LogigContent = logicContent;
            return this;
        }

        public IBuilderMethodDefinition Name(string name)
        {
            _Name = name;
            return this;
        }

        public IBuilderMethodDefinition Parameters(ImmutableList<Parameter>? parameters = null)
        {
            if (parameters == null)
            {
                _Parameters = "";
                return this;
            }

            string results = string.Join(", ", parameters.Select(x => string.Format("{0} {1}", x.Type, x.Name)).ToArray());
            _Parameters = results;

            return this;
        }
        /// <summary>
        /// The supress of parameter gen "public void" method
        /// </summary>
        /// <param name="returnDefinitions">Return configuration objet</param>
        /// <returns>Builder Object</returns>
        public IBuilderMethodDefinition ReturnDefinition(ReturnDefinition? returnDefinitions = null)
        {

            if (returnDefinitions == null)
            {
                _ReturnDefinition = "public void";
                return this;
            }

            _isConstructor = returnDefinitions.IsConstructor;

            switch (returnDefinitions.Type)
            {
                case null:
                    _ReturnDefinition = "public void";
                    return this;

                case "Void":
                    _ReturnDefinition = "void";
                    return this;

                default:
                    _ReturnDefinition = string.Format("{0} {1}",
                        (returnDefinitions.Visibility != Visibility.None) ? returnDefinitions.Visibility.ToString().ToLower() : ""
                        , returnDefinitions.Type);
                    return this;
            }
        }

        public IMethodDefinition InterfaceCreate()
        {
            IsNullOrEmptyCriticalFields();
            string parameters = (string.IsNullOrEmpty(_Parameters)) ? "()" : string.Format("({0})", _Parameters);
            string code = string.Format("{0} {1}{2};", _ReturnDefinition, _Name, parameters);
            return CreateMethod(code);
        }
    }
}
