using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Property
    {
        public Visibility Visibility { get; set; }
        public string? Name { get; set; }
        public string? TypeProperty { get; set; }
        public string? Attribuition { get; set; }

		public bool hasGeterAndSeter;
	}
}
