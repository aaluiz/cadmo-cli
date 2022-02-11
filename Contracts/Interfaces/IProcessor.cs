using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IProcessor
    {
        ImmutableList<string> GenerateCode();
        IFileBuilder FileBulder { get; }
    }
}
