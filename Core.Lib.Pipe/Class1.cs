using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Lib.Pipe
{
    internal class ActionPipeBuilder : IPipeBuilder<Action<IServiceProvider>>
    {
        private readonly IEnumerable<Delegate> _delegates;

        public ActionPipeBuilder(IEnumerable<Delegate> delegates)
        {
            _delegates = delegates;
        }
        public Action<IServiceProvider> Build()
            => throw new NotImplementedException();
    }

    internal class AsyncPipeBuilder : IPipeBuilder<Func<IServiceProvider, Task>>
    {
        private readonly IEnumerable<Delegate> _delegates;

        public AsyncPipeBuilder(IEnumerable<Delegate> delegates)
        {
            _delegates = delegates;
        }
        public Func<IServiceProvider, Task> Build()
            => throw new NotImplementedException();
    }

    public interface IPipeBuilder<TDelegate> where TDelegate : Delegate
    {
        TDelegate Build();
    }

    public static class Pipe
    {
        public static IPipeBuilder<Action<IServiceProvider>> Create(params Delegate[] delegates)
            => new ActionPipeBuilder(delegates);
    }

    public static class AsyncPipe
    {
        public static IPipeBuilder<Func<IServiceProvider, Task>> Create(params Delegate[] delegates)
            => new AsyncPipeBuilder(delegates);
    }
}
