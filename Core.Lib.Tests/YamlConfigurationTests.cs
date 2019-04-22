using Microsoft.Extensions.Configuration;
using Xunit;

namespace Core.Lib.Tests
{

    public class YamlConfigurationTests
    {
        [Fact]
        public void test_load_yaml_for_config()
        {
            //var config = new Matcher().AddInclude("**/*.yml").GetResultsInFullPath(Directory.GetCurrentDirectory())
            //        .Select(x => x.Replace(Directory.GetCurrentDirectory(), string.Empty))
            //        .Aggregate(
            //            (IConfigurationBuilder)new ConfigurationBuilder(),
            //            (cfg, path) => cfg.AddYaml(path, true, true))
            //        .Build();
            var config = new ConfigurationBuilder().AddYamlFile("assets\\lexer.yml").Build();
            var section = config.GetSection("operations:0:check:action");
            Assert.Equal("equals", section.Value);

        }
    }
}
