using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The method helper class.
    /// </summary>
    public static class MethodHelper
    {
        /// <summary>
        /// Get the format name.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetFormatName(this MethodInfo method)
            => $"{method.GetMethodName()}{method.GetMethodGenericArgumentName()}({method.GetMethodParameterNames()})";

        /// <summary>
        /// Get the method name.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetMethodName(this MethodInfo method)
            => method.IsGenericMethod ? method.GetGenericMethodDefinition().Name : method.Name;

        /// <summary>
        /// Get the method generic argument name.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetMethodGenericArgumentName(this MethodInfo method)
            => method.IsGenericMethod ? $"<{ string.Join(",", method.GetMethodGenericArgumentNames()) }>" : string.Empty;

        /// <summary>
        /// Get the method generic argument names.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="T:IEnumerable{string}"/>.</returns>
        private static IEnumerable<string> GetMethodGenericArgumentNames(this MethodInfo method)
            => method.IsGenericMethod ? method.GetGenericArguments().Select(TypeHelper.GetFormatName) : Enumerable.Empty<string>();

        /// <summary>
        /// Get the method parameter names.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetMethodParameterNames(this MethodInfo method)
            => string.Join(",", method.GetParameters().Select(x => x.ParameterType.GetFormatName()));

    }
}
