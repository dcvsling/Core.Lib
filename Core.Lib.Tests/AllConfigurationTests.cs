using Core.Lib.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Core.Lib.Tests
{
    public class AllConfigurationTests
    {
        [Fact]
        public void load_assets_setting_file_to_configuration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddSettings("assets", true, true)
                .Build();

            //Assert.Equal(1, config.GetSection("PropertyGroup").GetChildren().Count());
            Assert.Equal("Information", config.GetSection("Logging:Loglevel:Microsoft.Hosting.Lifetime").Value);
            Assert.Equal("Core.Net.OAuth;Core.Net.Rest", config.GetSection("HostingStartupAssemblies").Value);
        }
    }
}
