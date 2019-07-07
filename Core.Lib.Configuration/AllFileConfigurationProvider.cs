using Core.Lib.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
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
        private List<IConfigurationProvider> _snapshot;
        public AllFileConfigurationProvider(
            AllFileConfigurationSource source,
            Dictionary<string, Func<string, FileConfigurationProvider>> providers)
        {
            _providers = providers;
            _source = source;
            _snapshot = new List<IConfigurationProvider>();
        }

        public override void Load()
        {
            _source.ResolveFileProvider();
            _snapshot = _source.FileProvider.GetDirectoryContents(_source.Path)
                .Select(x => x)
                .Where(x => !x.IsDirectory
                    && x.Exists
                    && !new[] { ".exe", ".dll", ".pdb" }.Contains(Path.GetExtension(x.PhysicalPath)))
                .Select(CreateProvider)
                .ToList();
        }

        private IConfigurationProvider CreateProvider(IFileInfo file)
        {
            var ext = Path.GetExtension(file.PhysicalPath);
            var relatePath = file.PhysicalPath.Split(new[] {
                string.IsNullOrWhiteSpace(_source.Path) && _source.FileProvider is PhysicalFileProvider ppr
                    ? ppr.Root
                    : _source.Path
            }, StringSplitOptions.RemoveEmptyEntries).Last();

            var provider = _providers.ContainsKey(ext)
                ? _providers[ext].Invoke(relatePath)
                : new TextConfigurationProvider(relatePath);

            var prefix = _source.PrefixPattern.Value.Split(
                new[] { ConfigurationPath.KeyDelimiter },
                StringSplitOptions.RemoveEmptyEntries)
                .Select(x => KeywordMap.ContainsKey(x) ? KeywordMap[x].Invoke(file.PhysicalPath) : x)
                .ToJoin(ConfigurationPath.KeyDelimiter);
            try
            {
                provider.Load();
            }
            catch
            {
                // do something
            }
            return provider.PrependPrefix(prefix);
        }

        private static Dictionary<string, Func<string, string>> KeywordMap
            = new Dictionary<string, Func<string, string>> {
                ["{extension}"] = Path.GetExtension,
                ["{filename}"] = Path.GetFileNameWithoutExtension,
                ["{directoryname}"] = Path.GetDirectoryName,
            };


        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
            => _snapshot.SelectMany(x => x.GetChildKeys(earlierKeys, parentPath));

        public override bool TryGet(string key, out string value)
        {
            value = default;

            foreach (var provider in _snapshot)
            {
                if (provider.TryGet(key, out value))
                    return true;
            }
            return false;
        }
    }
}
