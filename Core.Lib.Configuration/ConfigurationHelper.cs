using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Core.Lib.Configuration
{
    internal static class ConfigurationHelper
    {
        public static TSource BindSource<TSource>(this TSource source, FileConfigurationSource origin)
            where TSource : FileConfigurationSource
        {
            source.Optional = origin.Optional;
            source.ReloadOnChange = origin.ReloadOnChange;
            source.ReloadDelay = origin.ReloadDelay;
            source.OnLoadException = origin.OnLoadException;
            source.FileProvider = origin.FileProvider;
            source.Path = origin.Path + source.Path.Split(new[] { origin.Path }, StringSplitOptions.RemoveEmptyEntries).Last();
            return source;
        }

        public static IConfigurationProvider PrependPrefix(this IConfigurationProvider provider, string prefix)
            => string.IsNullOrWhiteSpace(prefix)
                ? provider
                : new PrefixConfigurationProvider(new PrefixConfigurationSource { Prefix = prefix, Provider = provider });
    }
}
