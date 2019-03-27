using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Core.Lib.Module
{

    public interface IModuleStartup
    {
        void Configure(IConfiguration config, IServiceCollection services);
    }
}
