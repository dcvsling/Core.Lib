using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Rest.Abstractions;


namespace Microsoft.AspNetCore.Rest
{
    internal class InternalAssemblyLoadcontext : AssemblyLoadContext, IAssemblyLoadContext
    {
        public InternalAssemblyLoadcontext()
#if NETCOREAPP30
            : base(true)       
#endif
        {
        }

        protected override Assembly Load(AssemblyName assemblyName) => throw new System.NotImplementedException();
    }
}
