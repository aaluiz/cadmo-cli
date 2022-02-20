using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
	/// <summary>
	/// Contructor methods to implmente a source code c#
	/// </summary>
	public interface IBuilderClassDefinition
	{
		/// <summary>
		/// Return the using block of code
		/// </summary>
		/// <param name="imports"></param>
		/// <returns>Builder</returns>
		IBuilderClassDefinition Imports(ImmutableList<string> imports);
		/// <summary>
		/// Define Inhreritance blocks of code
		/// </summary>
		/// <param name="inheritances">List of class or intefaces that will be compose de child class</param>
		/// <returns>Builder</returns>
		IBuilderClassDefinition Inheritance(ImmutableList<string> inheritances);

		/// <summary>
		/// Define namespace, the package name
		/// </summary>
		/// <param name="namespaceName">Namespace</param>
		/// <returns>Builder</returns>        
		IBuilderClassDefinition Namespace(string namespaceName);
		/// <summary>
		/// Define Methods blocks of code
		/// </summary>
		/// <param name="methods"></param>
		/// <returns>Builder</returns>
		IBuilderClassDefinition Methods(ImmutableList<IMethodDefinition> methods);
		/// <summary>
		/// Define Properties
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		IBuilderClassDefinition Properties(ImmutableList<Property> properties);
		/// <summary>
		/// Define class name
		/// </summary>
		/// <param name="name">Class name</param>
		/// <returns>Builder/returns>
		IBuilderClassDefinition Name(string name, bool isInterface = false);
		/// <summary>
		/// Build Ideftinition
		/// </summary>
		/// <returns>Builder</returns>
		IClassDefinition Create();
	}
}
