using Contracts.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Abstractions
{
    /// <summary>
    /// Responseble for generate a series of classes like controllers, services and test of the services
    /// </summary>
    public abstract class CommandAbrstract : IGenerator
    {
        protected readonly INamespaceHandler? _namespaceHandler;
        protected readonly IClassDefinition? _classDefinition;
        protected readonly IMethodDefinition? _methodDefiniton;
        protected readonly IFileBuilder? _fileBuilder;
        protected readonly IPathManager? _pathManager;
        private readonly INamespaceHandler? namespaceHandler;
        private readonly IClassDefinition? classDefinition;
        private readonly IMethodDefinition? methodDefinition;
        private readonly IFileBuilder? fileBuilder;
        protected readonly ITypeProcessor? _typeProcessor;
        private IPathManager? pathManager;

        public abstract ImmutableList<FileCode> GenClasses();

        public CommandAbrstract(INamespaceHandler namespaceHandler,
                                                                IClassDefinition classDefinition,
                                                                IMethodDefinition methodDefinition,
                                                                IFileBuilder fileBuilder,
                                                                IPathManager pathManager,
                                                                ITypeProcessor typeProcessor)
        {
            _namespaceHandler = namespaceHandler;
            _classDefinition = classDefinition;
            _methodDefiniton = methodDefinition;
            _fileBuilder = fileBuilder;
            _pathManager = pathManager;
            _typeProcessor = typeProcessor;
        }

        protected CommandAbrstract(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder)
        {
            this.namespaceHandler = namespaceHandler;
            this.classDefinition = classDefinition;
            this.methodDefinition = methodDefinition;
            this.fileBuilder = fileBuilder;
        }

        public CommandAbrstract(INamespaceHandler namespaceHandler, IClassDefinition classDefinition, IMethodDefinition methodDefinition, IFileBuilder fileBuilder, IPathManager pathManager) : this(namespaceHandler, classDefinition, methodDefinition, fileBuilder)
        {
            this.pathManager = pathManager;
        }

        public abstract ImmutableList<FileCode> GenerateCode(ImmutableList<ClassElements> classElements);

        public abstract ImmutableList<ClassElements> GetConfigurationToClasses(ImmutableList<Type> types);

        public abstract ImmutableList<MethodElements> GetConfigurationToMethods(ImmutableList<MethodInfo> methods);

    }
}
