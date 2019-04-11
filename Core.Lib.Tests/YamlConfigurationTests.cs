using Core.Lib.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Core.Lib.Tests
{

    public class YamlConfigurationTests
    {
        [Fact]
        public void test_load_yaml_for_config()
        {
            var config = new ConfigurationBuilder().AddYaml("docker-compose.yml").Build();
            var section = config.GetSection("services:angular-starter:networks:0");
            Assert.Equal("angular-starter", section.Value);

        }
    }
}
