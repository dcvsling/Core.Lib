using Core.Lib.AppParts;
using Core.Lib.Reflections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Core.Lib.Tests
{
    public class AppPartTests
    {
        private static IServiceProvider Services
            => new ServiceCollection().AddApplicationParts()
                .AddSingleton<IApplicationFeatureProvider, TestFeatureProvider>()
                .BuildServiceProvider();

        [Fact]
        public void AddApplicationParts_will_inject_assemblies_as_ApplicationPart_to_di()
        {
            var service = Services;
            var manager = service.GetService<ApplicationPartManager>();
            Assert.NotEmpty(manager.ApplicationParts);
        }

        [Fact]
        public void AddApplicationParts_will_inject_feature_provider_to_di()
        {
            var service = Services;
            var manager = service.GetService<ApplicationPartManager>();
            Assert.NotEmpty(manager.FeatureProviders);
        }

        [Fact]
        public void populate_type_from_application_parts_manager()
        {
            var service = Services;
            var manager = service.GetService<ApplicationPartManager>();
            var feature = new TestFeature();
            manager.PopulateFeature(feature);
            Assert.NotEmpty(feature.Features);
        }

        public class TestFeature
        {
            public List<Type> Features { get; } = new List<Type>();
        }

        public class TestFeatureProvider : IApplicationFeatureProvider<TestFeature>
        {
            public void PopulateFeature(IEnumerable<ApplicationPart> parts, TestFeature feature)
                => parts.OfType<AssemblyPart>().Each(x => x.Types.Each(feature.Features.Add));
        }
    }
}
