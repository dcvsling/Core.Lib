using Core.Lib.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Xml;
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

    public class TextConfigurationProvider : FileConfigurationProvider
    {

        private readonly string _path;
        private readonly TextConfigurationSource _source = new TextConfigurationSource();
        public TextConfigurationProvider(string path) : base(new TextConfigurationSource())
        {
            _path = path;
        }
        public override void Load(Stream stream)
        {
            _source.ResolveFileProvider();
            var file = _source.FileProvider.GetFileInfo(_path);
            Data = file.Exists
                ? new Dictionary<string, string> { [_path] = File.ReadAllText(file.PhysicalPath) }
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
