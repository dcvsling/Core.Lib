using System.Collections.Generic;
using System.Reflection;

namespace Core.Lib.AppParts
{
    public interface IAssembliesProvider
    {
        IEnumerable<Assembly> ResolveAssemblies(Assembly entryAssembly);
    }
}