using Core.Lib.Helper;
using Microsoft.Extensions.Configuration;
using System;

namespace Core.Lib.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="KeyPerFileConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class AllFileConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds configuration using files from a directory. File names are used as the key,
        /// file contents are used as the value.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="directoryPath">The path to the directory.</param>
        /// <param name="optional">Whether the directory is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddSettings(this IConfigurationBuilder builder, string directoryPath, bool optional = true, bool reloadOnChange = true, string prefix = Empty.String)
            => builder.AddSettings(source =>
            {
                source.Optional = optional;
                source.ReloadOnChange = reloadOnChange;
                source.Path = directoryPath;
                source.PrefixPattern = prefix;
            });

        /// <summary>
        /// Adds configuration using files from a directory. File names are used as the key,
        /// file contents are used as the value.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddSettings(this IConfigurationBuilder builder, Action<AllFileConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
