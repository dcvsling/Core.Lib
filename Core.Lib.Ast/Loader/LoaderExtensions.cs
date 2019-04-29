using Core.Lib.Ast.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoaderExtensions
    {

        public static AstBuilder AddLoader(this AstBuilder builder)
        {
            builder.Services
                .AddSingleton<IConfiguration>(
                o => new Matcher().AddInclude("**/*.yml").GetResultsInFullPath(Directory.GetCurrentDirectory())
                    .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty))
                    .Aggregate(
                        (IConfigurationBuilder)new ConfigurationBuilder(),
                        (config, path) => config.AddYamlFile(path, true, true))
                    .Build());
            return builder;
        }
    }
}
