using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReturnDefinition
    {
        public Visibility Visibility { get; set; }
        public string? Type { get; set; }
        public bool IsConstructor { get; set; } = false;
    }
}
