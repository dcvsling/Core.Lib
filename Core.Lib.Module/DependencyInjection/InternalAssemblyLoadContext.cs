using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Core.Lib.Module.DependencyInjection
{
    public class InternalAssemblyLoadContext : AssemblyLoadContext, IEnumerable<Assembly>
    {
        private List<Assembly> _assemblies;
        public InternalAssemblyLoadContext()
#if NETCOREAPP30
                : base(true)
#endif
        {
            _assemblies = new List<Assembly>();
        }

        public InternalAssemblyLoadContext LoadByPath(string path)
        {
            _assemblies.Add(this.LoadFromAssemblyPath(path));
            return this;
        }
        public InternalAssemblyLoadContext LoadByStream(Stream stream)
        {
            _assemblies.Add(this.LoadFromStream(stream));
            return this;
        }
        public IEnumerator<Assembly> GetEnumerator() => ((IEnumerable<Assembly>)_assemblies).GetEnumerator();
        protected override Assembly Load(AssemblyName assemblyName) => default;
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Assembly>)_assemblies).GetEnumerator();
    }

    
}
