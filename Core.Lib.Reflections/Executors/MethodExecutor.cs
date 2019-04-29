using Core.Lib.Reflections.Executors;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Lib
{

    /// <summary>
    /// The method executor class.
    /// </summary>
    public static class MethodExecutor
    {
        /// <summary>
        /// Create the executor.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="IMethodExecutor"/>.</returns>
        public static IMethodExecutor CreateExecutor(this MethodInfo method)
            => new DefaultMethodExecutor(method);

        public static Task<object> ExecuteAsync(this IMethodExecutor executor, object target, params object[] args)
            => executor.Execute(target, args).CreateResult();

        private static Task<object> CreateResult(this object result)
            => result is Task
                ? result.GetType().IsGenericType
                    ? (Task<object>)result
                    : Task.FromResult<object>(default)
                : Task.FromResult(result);
    }
}
