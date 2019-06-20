using Microsoft.Extensions.Configuration;
using Xunit;

namespace Core.Lib.Tests
{

    public class YamlConfigurationTests
    {
        [Fact]
        public void test_load_yaml_for_config()
        {
            var config = new ConfigurationBuilder().AddYamlFile("assets/hostsettings.yml").Build();
            Assert.Equal("Core.Net.OAuth;Core.Net.Rest", config.GetSection("HostingStartupAssemblies").Value);

        }
    }
}
