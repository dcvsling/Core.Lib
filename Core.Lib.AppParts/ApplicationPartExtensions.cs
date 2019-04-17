// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Lib.Reflections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Mvc.ApplicationParts
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
                .AddSingleton(p => p.GetRequiredService<IOptions<ApplicationPartManager>>().Value);


    }
}
