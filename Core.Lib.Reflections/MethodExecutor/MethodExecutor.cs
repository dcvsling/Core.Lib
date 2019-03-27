using Core.Lib.MethodExecutor;
using System.Reflection;

namespace System
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
    }
}
