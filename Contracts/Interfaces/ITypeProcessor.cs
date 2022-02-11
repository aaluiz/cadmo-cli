using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ITypeProcessor
    {
        string MainDataTable { get; set; }
        string ReflectionToCode(Type type, bool returnType = false);
        /// <summary>
        /// Handle Types generate correct code representation
        /// </summary>
        /// <param name="type">Type that will be converted</param>
        /// <param name="offDataTableChecker">Active DataTableChecker to apply special treatement to DataTable Objects</param>
        /// <param name="returnType">Default is false, define if type is a returning type</param>
        /// <returns></returns>
        string ReflectionToCode(Type type, bool offDataTableChecker, bool returnType = false);
        /// <summary>
        /// Handle Types generate correct code representation
        /// </summary>
        /// <param name="type">Type that will be converted</param>
        /// <param name="offDataTableChecker">Active DataTableChecker to apply special treatement to DataTable Objects</param>
        /// <param name="returnType">Default is false, define if type is a returning type</param>
        /// <param name="refOrOut">Define if type is a ref or out, especialy check for parameters</param>
        /// <returns></returns>
        string ReflectionToCode(Type type, bool offDataTableChecker, bool returnType, string refOrOut);

    }
}
