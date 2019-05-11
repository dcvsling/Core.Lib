using Core.Lib.Reflections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Lib.Configuration
{


    internal class AllFileConfigurationProvider : ConfigurationProvider
    {
        private readonly Dictionary<string, Func<string, FileConfigurationProvider>> _providers;
        private readonly AllFileConfigurationSource _source;
        private Dictionary<string, FileConfigurationProvider> _snapshot;
        public AllFileConfigurationProvider(
            AllFileConfigurationSource source,
            Dictionary<string, Func<string, FileConfigurationProvider>> providers)
        {
            _providers = providers;
            _source = source;
            _snapshot = new Dictionary<string, FileConfigurationProvider>();
        }

        public override void Load()
        {
            _snapshot = new[] { ".exe", ".dll", "" }.Aggregate(
               new Matcher(),
               (seed, next) => seed.AddInclude($"**/*{next}"))
                   .GetResultsInFullPath(_source.Path)
                   .ToDictionary(
                       f => f.Replace(_source.Path, Empty.String)
                           .Replace(Path.GetExtension(f), Empty.String),
                       CreateProvider);
        }

        private FileConfigurationProvider CreateProvider(string path)
        {
            var ext = Path.GetExtension(path);
            return _providers.ContainsKey(ext)
                ? _providers[ext].Invoke(path)
                : new TextConfigurationProvider(_source.FileProvider, path);
        }
    }
}
