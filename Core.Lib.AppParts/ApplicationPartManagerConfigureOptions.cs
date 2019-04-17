using System.Collections.Generic;
using System.Linq;
using Core.Lib.Reflections;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Rest.Abstractions;
using Microsoft.Extensions.Options;


namespace Microsoft.AspNetCore.Rest
{

    internal class ApplicationPartManagerConfigureOptions : IConfigureNamedOptions<ApplicationPartManager>
    {
        private readonly IOptionsSnapshot<ApplicationFeature> _features;
        private readonly IEnumerable<IApplicationFeatureProvider> _providers;
        private readonly string _name;
        private readonly IAssemblyLoadContext _alc;

        public ApplicationPartManagerConfigureOptions(
            IOptionsSnapshot<ApplicationFeature> assemblyNames,
            IEnumerable<IApplicationFeatureProvider> providers,
            IAssemblyLoadContext alc)
            : this(string.Empty, assemblyNames, providers, alc)
        {
        }

        internal ApplicationPartManagerConfigureOptions(
            string name,
            IOptionsSnapshot<ApplicationFeature> features,
            IEnumerable<IApplicationFeatureProvider> providers,
            IAssemblyLoadContext alc)
        {
            _features = features;
            _providers = providers;
            _name = name;
            _alc = alc;
        }

        public void Configure(ApplicationPartManager options)
            => Configure(string.Empty, options);

        public void Configure(string name, ApplicationPartManager options)
        {
            var applicationPartManager = new ApplicationPartManager();
            _features.Get(name).AssemblyNames
                .Select(_alc.LoadFromAssemblyName)
                .Select(asm => new AssemblyPart(asm))
                .Each(applicationPartManager.ApplicationParts.Add);

            _providers.Each(options.FeatureProviders.Add);
        }
    }
}
