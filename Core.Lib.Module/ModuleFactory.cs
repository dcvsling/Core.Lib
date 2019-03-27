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

    internal sealed class ModuleFactory : IModuleFactory
    {
        private IOptionsMonitorCache<AssemblyLoadContext> _loader;
        private readonly IOptionsSnapshot<ModuleFeature> _feature;
        private readonly IOptionsMonitorCache<ServiceProvider> _cache;
        private readonly IConfiguration _configuration;

        public ModuleFactory(
            IOptionsMonitorCache<ServiceProvider> cache,
            IOptionsMonitorCache<AssemblyLoadContext> loader, 
            IOptionsSnapshot<ModuleFeature> feature,
            IConfiguration configuration)
        {
            _loader = loader;
            _feature = feature;
            _cache = cache;
            _configuration = configuration;
        }

        public ServiceExecutor CreateExecutor(string name)
            => new ServiceExecutor(
                _cache.GetOrAdd(name, CreateServiceProviderFactory(name))
                    .CreateScope().ServiceProvider);

        private Func<ServiceProvider> CreateServiceProviderFactory(string name)
            => () =>
            {
                var asms = _feature.Get(name).Assemblies;
                var alcs = asms.Aggregate(new InternalAssemblyLoadContext(),(alc,f) => f(alc)).ToArray();
                var attrs = alcs.SelectMany(x => x.GetCustomAttributes<ModuleStartupAttribute>()).ToArray();
                var objs = attrs.Select(x => Activator.CreateInstance(x.StartupType));
                var startups = objs.OfType<IModuleStartup>();
                var configs = startups.Aggregate((IServiceCollection)new ServiceCollection(), Configure);
                SetExportTypes(configs);
                var features = configs.AddSingleton(_feature);
                return features.BuildServiceProvider();
            };

        private IServiceCollection Configure(IServiceCollection services, IModuleStartup startup)
        {
            startup.Configure(_configuration, services);
            return services;
        }

        private IServiceCollection SetExportTypes(IServiceCollection services)
        {
            ExportTypes = services.Select(x => x.ServiceType).Distinct().ToArray();
            return services;
        }

        public IEnumerable<Type> ExportTypes { get; private set; }

        public void Dispose()
        {
            _cache.Clear();
#if NETCOREAPP30
            _loader.Unload();
#endif
            _loader = default;
            GC.SuppressFinalize(this);
        }
    }

}
