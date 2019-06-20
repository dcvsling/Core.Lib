using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Core.Lib.Reflections.Observables
{


    internal class MessageSwitch<TContext> : ISubject<TContext> where TContext : class
    {
        private readonly IOptionsMonitorCache<List<IObserver<TContext>>> _cache;
        private readonly List<IObserver<TContext>> _observers;
        public MessageSwitch(IOptionsMonitorCache<List<IObserver<TContext>>> cache)
        {
            _cache = cache;
            _observers = _cache.GetOrAdd(typeof(TContext).FullName, () => new List<IObserver<TContext>>());
        }
        public void OnCompleted()
            => _observers.ForEach(x => x.OnCompleted());
        public void OnError(Exception error)
            => _observers.ForEach(x => x.OnError(error));
        public void OnNext(TContext value)
            => _observers.ForEach(x => x.OnNext(value));
        public IDisposable Subscribe(IObserver<TContext> observer)
        {
            _observers.Add(observer);
            return new SubjectDisposer(() => _observers.Remove(observer));
        }


    }


}