using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Contracts.Interfaces;
using Models;

namespace Services.Abstract
{

    public abstract class AbstractGeneratorService
    {
        protected IClassDefinition? _classDefinition;
        protected IMethodDefinition? _methodDefinition;
        protected IFileBuilder? _fileBuilder;

        public AbstractGeneratorService(IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder)
        {
            _classDefinition = classDefinition;
            _methodDefinition = methodDefinition;
            _fileBuilder = fileBuilder;
        }

        protected static string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        protected static bool IsDefaultPath(string path)
        {
            return !(path.Contains('\\') || path.Contains('/'));
        }

        protected FileCode CreateBasicInterface(string name, string nameSpace)
        {
            var builder = _classDefinition!.Builder
                .Namespace(nameSpace)
                .Name(name)
                .Create();
            var result = new FileCode { Code = builder.ClassCode, FileName = $"{name}.cs" };

            return result;
        }

        protected static bool isWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        protected static bool isMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        protected static bool isLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}