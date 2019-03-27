using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Core.Lib.Module.DependencyInjection
{

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited =false)]
    public class ModuleStartupAttribute : Attribute
    {
        public ModuleStartupAttribute(Type startupType)
        {
            if (!typeof(IModuleStartup).IsAssignableFrom(startupType))
                throw new ArgumentException($"{nameof(startupType)} should assignable from {nameof(IModuleStartup)}");
            StartupType = startupType;
        }

        public Type StartupType { get; }
    }
}
