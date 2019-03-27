using Core.Lib.Module.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Lib.Module
{
    internal sealed class ModuleContext : IDisposable, IModuleContext
    {
        private static IOptionsMonitorCache<ServiceExecutor> _executors;
        private readonly IModuleFactory _factory;

        public ModuleContext(IModuleFactory factory, IOptionsMonitorCache<ServiceExecutor> executors)
        {
            _factory = factory;
            _executors = executors;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Invoke<TService>(string name, Action<TService> action) where TService : class
        {
            var hash = typeof(TService).GetHashCode();
            _executors.GetOrAdd(hash.ToString(), () => _factory.CreateExecutor(name))
                .Execute(action);
        }

        public void Dispose()
        {
            _executors.Clear();
            _executors = default;
            GC.SuppressFinalize(this);
        }
    }
}
