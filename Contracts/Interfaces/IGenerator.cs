using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    /// <summary>
    /// Contratct for classes generatos
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generate Colletion of C# classes content
        /// </summary>
        /// <returns>
        ///     Immutable List of C# generated code
        /// </returns>
        ImmutableList<FileCode> GenClasses();
        ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements);
        ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types);
        ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods);
    }
}
