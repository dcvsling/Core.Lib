using System;

namespace Core.Lib.Module
{
    public interface IModuleContext
    {
        void Invoke<TClient>(string name,Action<TClient> action) where TClient : class;
    }
}