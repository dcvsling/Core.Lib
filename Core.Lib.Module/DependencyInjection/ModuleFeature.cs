using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

namespace Core.Lib.Module.DependencyInjection
{

    public class ModuleFeature
    {
        public List<Func<InternalAssemblyLoadContext, InternalAssemblyLoadContext>> Assemblies { get; } = new List<Func<InternalAssemblyLoadContext, InternalAssemblyLoadContext>>();
    }
}
