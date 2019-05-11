﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Core.Lib.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="KeyPerFileConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class KeyPerFileConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds configuration using files from a directory. File names are used as the key,
        /// file contents are used as the value.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="directoryPath">The path to the directory.</param>
        /// <param name="optional">Whether the directory is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConfiguraionFiles(this IConfigurationBuilder builder, string directoryPath, bool optional, bool reloadOnChange)
            => builder.AddConfiguraionFiles(source =>
            {
                // Only try to set the file provider if its not optional or the directory exists 
                if (!optional || Directory.Exists(directoryPath))
                {
                    source.FileProvider = new PhysicalFileProvider(directoryPath);
                }
                source.Optional = optional;
                source.ReloadOnChange = reloadOnChange;
                source.Path = directoryPath;
            });

        /// <summary>
        /// Adds configuration using files from a directory. File names are used as the key,
        /// file contents are used as the value.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConfiguraionFiles(this IConfigurationBuilder builder, Action<AllFileConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
