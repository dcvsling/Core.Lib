using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Lib.Reflections.Mediator
{
    internal class Mediator<TContext> : IMediator<TContext> where TContext : class
    {
        private readonly ConcurrentDictionary<string, Func<TContext, Task>> _callbacks;
        public Mediator()
        {
            _callbacks = new ConcurrentDictionary<string, Func<TContext, Task>>();
        }

        public IDisposable Register(string id, Func<TContext, Task> callback)
        {
            _callbacks.AddOrUpdate(id, callback, (_, __) => callback);
            return new MediatorDisposer(() => _callbacks.TryRemove(id, out var func));
        }

        public Task Send(TContext context, string id = "")
            => (string.IsNullOrWhiteSpace(id)
                ? (ctx => _callbacks.Values.Aggregate(Task.FromResult(ctx), InvokeAsync))
                : _callbacks.ContainsKey(id)
                    ? _callbacks[id]
                    : throw new ArgumentException("key not found")).Invoke(context);

        async private Task<TContext> InvokeAsync(Task<TContext> context, Func<TContext, Task> callback)
        {
            var current = await context;
            await callback(current);
            return current;
        }

        private sealed class MediatorDisposer : IDisposable
        {
            private readonly Action _action;

            public MediatorDisposer(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
                GC.SuppressFinalize(this);
            }
        }
    }
}
