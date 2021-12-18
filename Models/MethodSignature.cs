using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MethodSignature
    {
        public Parameter? Parameter { get; set; }
        public ReturnDefinition? ReturnDefinition { get; set; }
        public Visibility? Visibility { get; set; }
        public string? Name { get; set; }
    }
}
