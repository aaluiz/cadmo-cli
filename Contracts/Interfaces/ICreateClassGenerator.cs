using Contracts.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
	public interface ICreateClassGenerator
	{
		public bool IsInterface { get; set; }
		public bool IsStatic { get; set; }
		FileCode CreateClass(string name, string nameSpace);
		FileCode CreateClass(string name, string nameSpace, ImmutableList<string> inheritance);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods);
		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<string> inheritance);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<string> inheritance);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<Property> properties, ImmutableList<string> inheritance);

		FileCode CreateClass(ImmutableList<string> imports, string name, string nameSpace, ImmutableList<IMethodDefinition> methods, ImmutableList<Property> properties, ImmutableList<string> inheritance);
	}
}
