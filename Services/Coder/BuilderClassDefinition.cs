using Contracts.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	[AddService]
	public class BuilderClassDefinition : IBuilderClassDefinition
	{
		private string? _Imports;
		private string? _Inheritances;
		private string? _Methods;
		private string? _Name;
		private string? _Namespace;
		private string? _Properties;

		public bool IsStatic { get; set; } = false;

		public IClassDefinition Create()
		{
			StringBuilder result = new StringBuilder();
			if (_Methods == null) Methods(null);

			_ = result.AppendLine(string.Format("{0}", _Imports).Trim());
			result.AppendLine("");
			result.AppendLine(string.Format("{0}", _Namespace).Trim());
			result.AppendLine("{");
			result.AppendLine(string.Format("    {0}{1}", _Name, _Inheritances).Trim());
			result.AppendLine("    {");
			if (!string.IsNullOrEmpty(_Properties))
			{
				result.AppendLine("");
				result.AppendLine(string.Format("       {0}", _Properties));
			}
			result.AppendLine("");
			result.AppendLine(string.Format("        {0}", _Methods));
			result.AppendLine("    }");
			result.AppendLine("}");

			Clean();

			return new ClassDefinition(this, result.ToString());
		}

		private void Clean()
		{
			_Imports = null;
			_Inheritances = null;
			_Methods = null;
			_Name = null;
			_Namespace = null;
			_Properties = null;
		}

		public IBuilderClassDefinition Imports(ImmutableList<string> imports)
		{
			_Imports = string.Join("\n", imports.Select(x => string.Format("using {0};", x)).ToArray()).Trim();
			return this;
		}

		public IBuilderClassDefinition Inheritance(ImmutableList<string> inheritances)
		{
			_Inheritances = ": " + string.Join(", ", inheritances.Select(x => x).ToArray()).Trim();
			return this;
		}

		public IBuilderClassDefinition Methods(ImmutableList<IMethodDefinition>? methods)
		{
			if (methods == null)
			{
				_Methods = "";
				return this;
			}
			_Methods = string.Join("\n", methods.Select(x => "        " + x.MethodCode).ToArray()).Trim();
			return this;
		}

		public IBuilderClassDefinition Name(string name, bool isInterface = false)
		{
			string type = (isInterface) ? "interface" : "class";
			if (IsStatic)
			{

				_Name = string.Format("public static {0} {1}", type, name).Trim();
			}
			else
			{
				_Name = string.Format("public {0} {1}", type, name).Trim();
			}
			return this;
		}

		public IBuilderClassDefinition Namespace(string namespaceName)
		{
			_Namespace = string.Format("namespace {0}", namespaceName).Trim();
			return this;
		}

		private string HandleVisibility(Property property)
		{
			switch (property.Visibility)
			{
				case Visibility.Public:
					return "public";
				case Visibility.Protected:
					return "protected";
				case Visibility.Private:
					return "private";
				case Visibility.None:
					return "";
				default:
					return "public";
			}
		}

		public IBuilderClassDefinition Properties(ImmutableList<Property> properties)
		{
			_Properties = string.Join("\n", properties.Select(x =>
			{
				if (x.hasGeterAndSeter)
				{
					if (!string.IsNullOrEmpty(x.Annotations))
					{
						return string.Format("{0} {1} {2} {3} {{get; set;}}", x.Annotations, HandleVisibility(x), x.TypeProperty, x.Name);
					}
					return string.Format("{0} {1} {2} {{get; set;}}", HandleVisibility(x), x.TypeProperty, x.Name);
				}

				if (!string.IsNullOrEmpty(x.Attribuition))
				{
					return string.Format("{0} {1} {2} ={3};", HandleVisibility(x), x.TypeProperty, x.Name, x.Attribuition);
				}
				return string.Format("{0} {1} {2};", HandleVisibility(x), x.TypeProperty, x.Name, x.Attribuition); ;
			}));
			return this;
		}

	}
}
