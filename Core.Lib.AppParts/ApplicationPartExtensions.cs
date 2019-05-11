// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Core.Lib.Reflections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Lib.AppParts
{
    public static class ApplicationPartExtensions
    {
        public static IServiceCollection AddApplicationParts(this IServiceCollection services)
            => services
                .AddApplicationPartManagerService()
                .AddSingleton(AddAssembliesIntoServiceProvider)
                .AddDefaultAssemblyParts();

        private static IServiceCollection AddApplicationPartManagerService(this IServiceCollection services)
            => services
                .AddSingleton(p => p.GetRequiredService<IOptions<ApplicationPartManager>>().Value)
                .AddSingleton<IAssemblyLoadContext, InternalAssemblyLoadcontext>()
                .ConfigureOptions<ApplicationPartManagerConfigureOptions>();

        public static IServiceCollection AddParts(this IServiceCollection services, params ApplicationPart[] parts)
            => services
                .Configure<ApplicationPartManager>(m => parts.Each(m.ApplicationParts.Add));

        public static IServiceCollection AddAssemblyParts(this IServiceCollection services, params AssemblyName[] assemblies)
            => services.Configure<ApplicationFeature>(
                feature => feature.AssemblyNames.AddRange(assemblies));

        private static IServiceCollection AddDefaultAssemblyParts(this IServiceCollection services)
            => services.AddAssemblyParts(
                assemblies: DependencyContext
                .Default
                .GetDefaultAssemblyNames()
                .ToArray());

        private static IEnumerable<ApplicationPart> AddAssembliesIntoServiceProvider(IServiceProvider provider)
        {
            var alc = provider.GetRequiredService<IAssemblyLoadContext>();
            return provider.GetRequiredService<IOptions<ApplicationFeature>>().Value
                .AssemblyNames
                .Select(alc.LoadFromAssemblyName)
                .SelectMany(ApplicationPartFactory.GetParts)
                .OfType<ApplicationPart>()
                .ToArray();

        }
    }
}
