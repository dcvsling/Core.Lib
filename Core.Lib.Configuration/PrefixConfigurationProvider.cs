using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Core.Lib.Configuration
{
    public class PrefixConfigurationProvider : IConfigurationProvider
    {
        private readonly PrefixConfigurationSource _source;

        public PrefixConfigurationProvider(PrefixConfigurationSource source)
        {
            _source = source;
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
            => _source.Provider.GetChildKeys(earlierKeys, _source.Prefix + parentPath);
        public IChangeToken GetReloadToken() => _source.Provider.GetReloadToken();
        public void Load() => _source.Provider.Load();
        public void Set(string key, string value) => _source.Provider.Set(
            _source.Prefix + key,
            value);
        public bool TryGet(string key, out string value)
            => _source.Provider.TryGet(_source.Prefix + ConfigurationPath.KeyDelimiter + key, out value);
    }
}
