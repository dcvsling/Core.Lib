using Core.Lib.Reflections;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.AppParts
{

    internal class ApplicationPartManagerConfigureOptions : IConfigureOptions<ApplicationPartManager>
    {
        private readonly IOptions<ApplicationFeature> _features;
        private readonly IEnumerable<IApplicationFeatureProvider> _providers;
        private readonly IAssemblyLoadContext _alc;

        public ApplicationPartManagerConfigureOptions(
            IOptions<ApplicationFeature> features,
            IEnumerable<IApplicationFeatureProvider> providers,
            IAssemblyLoadContext alc)
        {
            _features = features;
            _providers = providers;
            _alc = alc;
        }

        public void Configure(ApplicationPartManager options)
        {
            _features.Value.AssemblyNames
                .Select(_alc.LoadFromAssemblyName)
                .Select(asm => new AssemblyPart(asm))
                .Each(options.ApplicationParts.Add);

            _providers.Each(options.FeatureProviders.Add);
        }
    }
}
