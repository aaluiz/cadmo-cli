
using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Abstractions
{
	/// <summary>
	/// Abstract Classs for the definition of struture that will be generate a c# class code content
	/// </summary>
	public abstract class ClassDefinerAbstract : IClassDefinition
	{
		private IBuilderClassDefinition? builder;

		/// <summary>
		/// Show the code content for one c# class
		/// </summary>
		public abstract string ClassCode { get; }

		/// <summary>
		/// Builder property responsable for generate code elements like usings, namespaces and signatures
		/// </summary>
		/// 
		public IBuilderClassDefinition Builder
		{
			get => builder
				?? throw new InvalidOperationException("Uninitialized property: " + nameof(IBuilderClassDefinition)); 
			set => builder = value;
		}
	}
}
