using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Core.Lib.Reflections.Mediator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
            => services.AddOptions()
                .AddSingleton<MediatorStore>();
    }


    public class MediatorStore
    {
        private readonly IOptionsMonitorCache<IMediator> _cache;

        public MediatorStore(IOptionsMonitorCache<IMediator> cache)
        {
            _cache = cache;
        }
        public IDisposable Register<TContext>(string id, Func<TContext, Task> callback)
            where TContext : class
            => GetMediator<TContext>().Register(id, callback);

        public Task Send<TContext>(TContext context, string id = "")
            where TContext : class
            => GetMediator<TContext>().Send(context, id);

        private IMediator<TContext> GetMediator<TContext>() where TContext : class
            => (IMediator<TContext>)_cache.GetOrAdd(typeof(TContext).FullName, () => new Mediator<TContext>());
    }
}
