using System;

namespace Core.Lib.Reflections.Observables
{
    public interface IMediator : IDisposable
    {
        void Send<TContext>(TContext value) where TContext : class;
        void Subscribe<TCallback>(Action<TCallback> callback) where TCallback : class;
    }
}