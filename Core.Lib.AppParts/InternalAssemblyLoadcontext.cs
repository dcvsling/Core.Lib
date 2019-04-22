using Microsoft.AspNetCore.Rest.Abstractions;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Microsoft.AspNetCore.Rest
{
    internal class InternalAssemblyLoadcontext : IAssemblyLoadContext
    {
        private readonly AssemblyLoadContext _alc
            = AssemblyLoadContext.Default;

        public Assembly LoadFromAssemblyName(AssemblyName assemblyName)
            => _alc.LoadFromAssemblyName(assemblyName);
        public Assembly LoadFromAssemblyPath(string assemblyPath)
            => _alc.LoadFromAssemblyPath(assemblyPath);
        public Assembly LoadFromStream(Stream assembly)
            => _alc.LoadFromStream(assembly);
        public Assembly LoadFromStream(Stream assembly, Stream assemblySymbols)
            => _alc.LoadFromStream(assembly, assemblySymbols);
    }
}
