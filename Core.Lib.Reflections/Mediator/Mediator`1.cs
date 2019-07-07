using Core.Lib.Helper;
using Core.Lib.Reflections.Observables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Core.Lib.Reflections.Mediator
{

    internal class DefaultMediator : IMediator
    {
        private readonly IObservable _observable;
        private readonly Dictionary<Type, IDisposable> _disposers;
        private readonly IServiceProvider _services;

        public DefaultMediator(IServiceProvider services, IObservable observable)
        {
            _observable = observable;
            _disposers = new Dictionary<Type, IDisposable>();
            _services = services;
        }

        public void Dispose() => _disposers.Values.Each(x => x.Dispose());

        public void Send<TContext>(TContext value) where TContext : class
        {
            var observer = _services.GetRequiredService<IObserver<TContext>>();
            try
            {
                observer.OnNext(value);
            }
            catch (Exception ex)
            {
                observer.OnError(ex);
            }
        }

        public void Subscribe<T>(Action<T> callback) where T : class
            => _disposers.Add(typeof(T), _observable.Subscribe(callback));
    }


}