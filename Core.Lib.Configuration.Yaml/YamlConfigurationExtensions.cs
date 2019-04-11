using Core.Lib.Configuration.Yaml;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Microsoft.Extensions.Configuration
{

    public static class YamlConfigurationExtensions
    {
        public static IConfigurationBuilder AddYaml(this IConfigurationBuilder builder, string path, bool options = true, bool reload = true)
            => builder.Add(new YamlConfigurationSource {
                Path = path,
                ReloadOnChange = reload,
                Optional = options,
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });
    }
}
