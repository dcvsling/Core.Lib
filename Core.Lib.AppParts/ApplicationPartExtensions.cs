// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Core.Lib.Reflections;
using Microsoft.AspNetCore.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Lib.AppParts
{
    public static class ApplicationPartExtensions
    {
        public static IServiceCollection AddApplicationParts(this IServiceCollection services)
            => services.AddSingleton<ApplicationPartFactory>(_ => DefaultApplicationPartFactory.Instance)
                .AddOptions<ApplicationPartManager>()
                    .Configure<IEnumerable<IApplicationFeatureProvider>>(
                        (m, ps) => ps.Each(m.FeatureProviders.Add))
                    .Configure<IEnumerable<ApplicationPart>>(
                        (m, ps) => ps.Each(m.ApplicationParts.Add))
                .Services
                .AddSingleton(p => p.GetRequiredService<IOptions<ApplicationPartManager>>().Value)

                .AddDefaultAssemblyParts();

        private static IServiceCollection AddParts(this IServiceCollection services, string name = Empty.String, params AssemblyName[] assemblies)
            => services.Configure<ApplicationFeature>(
                name,
                feature => feature.AssemblyNames.AddRange(assemblies));

        private static IServiceCollection AddDefaultAssemblyParts(this IServiceCollection services)
            => services.AddParts(
                name: Empty.String,
                assemblies: DependencyContext
                .Default
                .GetDefaultAssemblyNames()
                .ToArray());
    }
}
