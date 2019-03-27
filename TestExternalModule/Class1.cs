using Core.Lib.Module;
using Core.Lib.Module.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestModule;

[assembly: ModuleStartup(typeof(ExternalTestModule.ModuleStartup))]
namespace ExternalTestModule
{
    public class ModuleStartup : IModuleStartup
    {
        public void Configure(IConfiguration _, IServiceCollection services)
        {
            services.AddSingleton<IModuleService, ModuleService>();
        }
    }

    public class ModuleService : IModuleService
    {
        public bool Assert(object obj)
            => obj.GetHashCode() != new object().GetHashCode();
    }
}