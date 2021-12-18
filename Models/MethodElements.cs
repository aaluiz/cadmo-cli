using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MethodElements
    {
        public string? Name { get; set; }
        public ImmutableList<Parameter>? Parameters { get; set; }
        public ImmutableList<string>? Annotations { get; set; }
        public ReturnDefinition? ReturnDefinition { get; set; }
        public string? LogicContent { get; set; }
        public bool isInterface { get; set; } = false;
    }
}
