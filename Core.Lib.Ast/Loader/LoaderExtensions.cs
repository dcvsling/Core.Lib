using System.IO;
using System.Linq;
using Core.Lib.Ast.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoaderExtensions
    {

        public static AstBuilder AddLoader(this AstBuilder builder)
        {
            builder.Services
                .ConfigureOptions<AstOptionsConfigureOptions>()
                .AddSingleton<IConfiguration>(
                o => new Matcher().AddInclude("**/*.yml").GetResultsInFullPath(Directory.GetCurrentDirectory())
                    .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty))
                    .Aggregate(
                        (IConfigurationBuilder)new ConfigurationBuilder(),
                        (config, path) => config.AddYaml(path, true, true))
                    .Build());
            return builder;
        }
    }
}
