using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;

namespace Core.Lib.Reflections.Observables
{
    internal class Observable : IObservable
    {
        private readonly IServiceProvider _services;
        private readonly IObserver<Exception> _errorListener;
        public Observable(IServiceProvider services)
        {
            _services = services;
            _errorListener = _services.GetRequiredService<IObserver<Exception>>();
        }

        public IDisposable Subscribe<TContext>(Action<TContext> action)
            => _services.GetRequiredService<IObservable<TContext>>()
                .Subscribe(new AnonymousObserver<TContext>(action, _errorListener.OnNext));
    }


}