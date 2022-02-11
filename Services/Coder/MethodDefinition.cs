using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [AddService]
    public class MethodDefinition : IMethodDefinition
    {
        private string _methodCode;

        public string MethodCode
        {
            get { return _methodCode; }
        }

        private IBuilderMethodDefinition _builder;

        public IBuilderMethodDefinition Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        public MethodDefinition(IBuilderMethodDefinition builder, string? methodCode = null)
        {
            _builder = builder;
            _methodCode = (methodCode != null) ? methodCode : "";
        }
    }
}
