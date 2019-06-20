using Microsoft.Extensions.Configuration;

namespace Core.Lib.Configuration
{
    public class PrefixConfigurationSource : IConfigurationSource
    {
        public string Prefix { get; set; }
        public IConfigurationProvider Provider { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => new PrefixConfigurationProvider(this);
    }
}
