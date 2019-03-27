using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Reflections
{
    /// <summary>
    /// The type helper class.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Get the format name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetFormatName(this Type type)
            => $"{type.GetTypeName()}{type.GetGenericArgumentName() }";

        /// <summary>
        /// Get the type name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetTypeName(this Type type)
            => (type.IsGenericType ? type.GetGenericTypeDefinition() : type).Name;

        /// <summary>
        /// Get the generic argument name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetGenericArgumentName(this Type type)
            => type.IsGenericType ? $"<{ string.Join(",", type.GetGenericArgumentNames()) }>" : string.Empty;

        /// <summary>
        /// Get the generic argument names.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="T:IEnumerable{string}"/>.</returns>
        private static IEnumerable<string> GetGenericArgumentNames(this Type type)
            => type.IsGenericType ? type.GenericTypeArguments.Select(GetFormatName) : Enumerable.Empty<string>();



        /// <summary>
        /// The is value type or string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsValueTypeOrString(this Type type)
            => type.IsValueType || type == typeof(string);

        /// <summary>
        /// The has non parameter constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool HasNonParameterConstructor(this Type type)
            => type.GetConstructors().Any(ctor => ctor.IsPublic && !ctor.GetParameters().Any());

        /// <summary>
        /// Get the format full name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetFormatFullName(this Type type)
            => $"{type.Namespace}.{type.GetFormatName()}";
    }
}
