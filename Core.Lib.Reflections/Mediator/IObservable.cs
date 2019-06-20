using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Core.Lib.Reflections.Observables
{



    internal interface IObservable
    {
        IDisposable Subscribe<TContext>(Action<TContext> action);
    }


}