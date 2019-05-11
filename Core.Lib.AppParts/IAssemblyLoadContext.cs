using System.IO;
using System.Reflection;

namespace Core.Lib.AppParts
{

    public interface IAssemblyLoadContext
    {
        Assembly LoadFromAssemblyName(AssemblyName assemblyName);

        Assembly LoadFromAssemblyPath(string assemblyPath);

        Assembly LoadFromStream(Stream assembly);

        Assembly LoadFromStream(Stream assembly, Stream assemblySymbols);

    }
}
