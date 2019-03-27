using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Module
{
    internal interface IModuleFactory : IDisposable
    {
        IEnumerable<Type> ExportTypes { get; }

        ServiceExecutor CreateExecutor(string name);
    }
}