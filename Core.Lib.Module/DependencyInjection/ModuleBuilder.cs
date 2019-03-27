using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Core.Lib.Module.DependencyInjection
{

    public class ModuleBuilder
    {
        public ModuleBuilder(string name, IServiceCollection services, IConfiguration configuration)
        {
            Name = name;
            Services = services;
            Configuration = configuration;
        }

        public string Name { get; }
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public ModuleBuilder AddAssembly(params Func<InternalAssemblyLoadContext, InternalAssemblyLoadContext>[] assemblies)
        {
            Services.Configure<ModuleFeature>(Name, feature => feature.Assemblies.AddRange(assemblies));
            
            return this;
        }
   
        internal ModuleBuilder AddCoreService()
        {
            if (Services.Any(x => x.ServiceType.Equals(typeof(ModuleMarkup)))) return this;

            Services.AddScoped<IModuleContext, ModuleContext>()
                .AddSingleton<IModuleFactory, ModuleFactory>()
                .AddSingleton(Configuration)
                .Insert(0, ServiceDescriptor.Singleton(new ModuleMarkup()));
            return this;
        }

        private class ModuleMarkup { }
    }

    
}
