using Core.Lib.AppParts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ESI.Core.Tests
{
    public class AppPartTests
    {
        [Fact]
        public void AddApplicationParts_will_inject_assemblies_as_ApplicationPart_to_di()
        {
            var service = new ServiceCollection().AddApplicationParts().BuildServiceProvider();
            var assemblies = service.GetServices<ApplicationPart>();
            Assert.NotEmpty(assemblies);
        }
    }
}
