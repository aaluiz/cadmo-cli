using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClassElements
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public ImmutableList<MethodElements>? Methods { get; set; }
        public ImmutableList<string>? Imports { get; set; }
        public ImmutableList<string>? Inheritance { get; set; }
        public ImmutableList<Property>? Properties { get; set; }
    }
}
