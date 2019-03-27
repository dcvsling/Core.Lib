using Core.Lib.Module.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    internal sealed class ServiceExecutor : IServiceExecutor, IAsyncServiceExecutor
    {
        private IServiceProvider _provider;
        public ServiceExecutor(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Dispose()
        {
            (_provider is IDisposable disposable
                    ? disposable.Dispose
                    : (Action)(() => { }))
                .Invoke();
            _provider = default;
            GC.SuppressFinalize(this);
        }
        public void Execute<T>(Action<T> action)
            => action(_provider.GetService<T>());
        public Task ExecuteAsync<T>(Func<T, Task> func)
            => func(_provider.GetService<T>());
    }

}
