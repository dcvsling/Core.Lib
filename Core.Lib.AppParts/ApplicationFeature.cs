using System.Collections.Generic;
using System.Reflection;


namespace Microsoft.AspNetCore.Rest
{

    internal class ApplicationFeature
    {
        public List<AssemblyName> AssemblyNames { get; } = new List<AssemblyName>();
    }
}
