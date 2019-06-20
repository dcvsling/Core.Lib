using Core.Lib.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Xml;
using System;
using System.Collections.Generic;

namespace Core.Lib.Configuration
{

    /// <summary>
    /// An <see cref="IConfigurationSource"/> used to configure <see cref="KeyPerFileConfigurationProvider"/>.
    /// </summary>
    public class AllFileConfigurationSource : FileConfigurationSource
    {
        /// <summary>
        /// Constructor;
        /// </summary>
        public AllFileConfigurationSource()
            => IgnoreCondition = s => IgnorePrefix != null && s.StartsWith(IgnorePrefix);

        public StringPattern PrefixPattern { get; set; } = StringPattern.NoPrefix;

        /// <summary>
        /// Files that start with this prefix will be excluded.
        /// Defaults to "ignore.".
        /// </summary>
        public string IgnorePrefix { get; set; } = "ignore.";

        /// <summary>
        /// Used to determine if a file should be ignored using its name.
        /// Defaults to using the IgnorePrefix.
        /// </summary>
        public Func<string, bool> IgnoreCondition { get; set; }

        /// <summary>
        /// Builds the <see cref="KeyPerFileConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="KeyPerFileConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = builder.GetFileProvider();

            return new AllFileConfigurationProvider(this, _providers);
        }

        private Dictionary<string, Func<string, FileConfigurationProvider>> _providers
            => new Dictionary<string, Func<string, FileConfigurationProvider>> {
                [".json"] = path => new JsonConfigurationProvider(
                    new JsonConfigurationSource { Path = path }.BindSource(this)),
                [".yml"] = path => new YamlConfigurationProvider(
                    new YamlConfigurationSource { Path = path }.BindSource(this)),
                [".xml"] = path => new XmlConfigurationProvider(
                    new XmlConfigurationSource { Path = path }.BindSource(this)),
                [".ini"] = path => new IniConfigurationProvider(
                    new IniConfigurationSource { Path = path }.BindSource(this)),
                [".txt"] = path => new TextConfigurationProvider(path),
            };



    }
}
