using System.IO;
using System.Reflection;

namespace Microsoft.AspNetCore.Rest.Abstractions
{

    public interface IAssemblyLoadContext
    {
        Assembly LoadFromAssemblyName(AssemblyName assemblyName);

        Assembly LoadFromAssemblyPath(string assemblyPath);

        Assembly LoadFromStream(Stream assembly);

        Assembly LoadFromStream(Stream assembly, Stream assemblySymbols);

    }
}
