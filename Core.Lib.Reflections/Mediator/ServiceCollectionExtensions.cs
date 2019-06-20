using Core.Lib.Reflections.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Lib.Reflections.Observables
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubject(this IServiceCollection services)
            => services.AddOptions()
                .AddSingleton(typeof(IObserver<>), typeof(MessageSwitch<>))
                .AddSingleton(typeof(IObservable<>), typeof(MessageSwitch<>))
                .AddScoped(typeof(IMediator), typeof(DefaultMediator))
                .AddSingleton<IObservable, Observable>();

    }
}
