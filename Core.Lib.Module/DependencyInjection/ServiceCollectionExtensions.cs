using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Core.Lib.Module.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ModuleBuilder AddModule(this IServiceCollection services,string name, IConfiguration config = default)
            => new ModuleBuilder(name, services, config ?? new ConfigurationBuilder().Build()).AddCoreService();
        
        public static ModuleBuilder AddAssembly(this ModuleBuilder builder,string root, string pattern)
            => builder.AddAssembly(new Matcher()
                .AddInclude(pattern)
                .GetResultsInFullPath(Path.Combine(Directory.GetCurrentDirectory(),root))
                .Select(LoadByPath).ToArray());

        private static Func<InternalAssemblyLoadContext, InternalAssemblyLoadContext> LoadByPath(string path)
            => ctx => ctx.LoadByPath(path);
        private static Func<InternalAssemblyLoadContext, InternalAssemblyLoadContext> LoadByStream(Action<Stream> action)
            => ctx =>
            {
                using (var stream = new MemoryStream())
                {
                    action(stream);
                    return ctx.LoadByStream(stream);
                }
            };

        public static ModuleBuilder AddAssembly(this ModuleBuilder builder, Action<Stream> action)
            => builder.AddAssembly(LoadByStream(action));

    }
}
