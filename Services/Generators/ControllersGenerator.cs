using System.Collections.Immutable;
using System.Reflection;
using Contracts.Interfaces;
using Models;

namespace Services.Generators
{
	[AddService]
	public class ControllerGenerator : IControllerGenerator
	{
		public ImmutableList<FileCode> GenClasses()
		{
			throw new NotImplementedException();
		}

		public ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements)
		{
			throw new NotImplementedException();
		}

		public ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types)
		{
			throw new NotImplementedException();
		}

		public ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods)
		{
			throw new NotImplementedException();
		}
	}
}