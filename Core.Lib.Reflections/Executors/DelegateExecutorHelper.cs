using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Lib.Reflections.Executors
{

    public static class DelegateExecutorHelper
    {
        private static ConcurrentDictionary<IMethodExecutor, object> _caches
            = new ConcurrentDictionary<IMethodExecutor, object>();

        public static object Invoke<TDelegate>(this TDelegate @delegate, params object[] args) where TDelegate : Delegate
            => MethodExecutor.CreateExecutor(@delegate.Method).Execute(args);

        public static TDelegate Wrap<TDelegate>(this IMethodExecutor executor, object target)
            => (TDelegate)_caches.GetOrAdd(executor, exec =>
            {
                var method = exec.MethodInfo;
                var parameters = exec.MethodParameters.Select(p => Expression.Parameter(p.ParameterType)).ToArray();
                var invoke = Expression.Call(Expression.Constant(target), method, parameters);
                var runner = method.ReturnType == typeof(void) ? (Expression)Expression.Block(invoke) : Expression.Convert(invoke, method.ReturnType);
                return Expression.Lambda<TDelegate>(runner, parameters).Compile();

            });
    }
}
