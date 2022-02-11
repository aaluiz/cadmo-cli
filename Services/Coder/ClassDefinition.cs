using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [AddService]
    public class ClassDefinition : IClassDefinition
    {
        public string ClassCode { get; }

        private IBuilderClassDefinition _builder;

        public IBuilderClassDefinition Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        public ClassDefinition(IBuilderClassDefinition builder, string? classCode = null)
        {
            _builder = builder;
            ClassCode = (classCode != null) ? classCode : "";
        }
    }
}
