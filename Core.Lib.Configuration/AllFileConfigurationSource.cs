using Core.Lib.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Xml;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Lib.Configuration
{
#if NETCOREAPP30
    using JsonConfigurationProvider = Microsoft.Extensions.Configuration.NewtonsoftJson.NewtonsoftJsonConfigurationProvider;
    using JsonConfigurationSource = Microsoft.Extensions.Configuration.NewtonsoftJson.NewtonsoftJsonConfigurationSource;
#else
    using JsonConfigurationProvider = Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider;
    using JsonConfigurationSource = Microsoft.Extensions.Configuration.Json.JsonConfigurationSource;
#endif

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

        /// <summary>
        /// The FileProvider whos root "/" directory files will be used as configuration data.
        /// </summary>
        public IFileProvider FileProvider { get; set; }

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
            => new AllFileConfigurationProvider(this, _providers);

        private Dictionary<string, Func<string, FileConfigurationProvider>> _providers
            => new Dictionary<string, Func<string, FileConfigurationProvider>> {
                [".json"] = path => new JsonConfigurationProvider(
                    BindSource(new JsonConfigurationSource { Path = path })),
                [".yml"] = path => new YamlConfigurationProvider(
                    BindSource(new YamlConfigurationSource { Path = path })),
                [".xml"] = path => new XmlConfigurationProvider(
                    BindSource(new XmlConfigurationSource { Path = path })),
                [".ini"] = path => new IniConfigurationProvider(
                    BindSource(new IniConfigurationSource { Path = path })),
                [".txt"] = path => new TextConfigurationProvider(FileProvider, path),
            };

        private TSource BindSource<TSource>(TSource source) where TSource : FileConfigurationSource
        {
            source.Optional = Optional;
            source.FileProvider = FileProvider;
            source.ReloadOnChange = ReloadOnChange;
            source.ReloadDelay = ReloadDelay;
            source.OnLoadException = OnLoadException;
            return source;
        }
    }

    public class TextConfigurationProvider : FileConfigurationProvider
    {

        private readonly string _path;
        private readonly IFileProvider _provider;
        private readonly TextConfigurationSource _source = new TextConfigurationSource();
        public TextConfigurationProvider(IFileProvider provider, string path) : base(new TextConfigurationSource())
        {
            _path = path;
            _provider = provider;
            ((TextConfigurationSource)Source).Provider = this;
        }
        public override void Load(Stream stream)
        {
            var path = _provider.GetFileInfo(_path).PhysicalPath;
            Data = File.Exists(path)
                ? new Dictionary<string, string> { [_path] = File.ReadAllText(path) }
                : new Dictionary<string, string>();
        }

        internal class TextConfigurationSource : FileConfigurationSource
        {
            public TextConfigurationProvider Provider { get; internal set; }

            public override IConfigurationProvider Build(IConfigurationBuilder builder)
                => Provider;
        }
    }
}
