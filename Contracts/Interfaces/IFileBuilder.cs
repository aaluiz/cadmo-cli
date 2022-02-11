using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IFileBuilder
    {
        bool WriteFiles(ImmutableList<FileCode> contents, string path);
		bool WriteFile(FileCode filecode, string path);
	}
}
