using System;
using System.Threading.Tasks;

namespace Core.Lib.Mediator
{
    public interface IMediator<TContext> : IMediator where TContext : class
    {
        IDisposable Register(string id, Func<TContext, Task> callback);
        Task Send(TContext context, string id = "");
    }
}