using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The optional helper class.
    /// </summary>
    public static class OptionalHelper
    {
        /// <summary>
        /// Create the hash code.
        /// </summary>
        /// <param name="seq">The seq.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>The <see cref="int"/>.</returns>
        /// <typeparam name="T"></typeparam>
        internal static int CreateHashCode<T>(this IEnumerable<T> seq, int seed)
            => seq.Aggregate(seed, (left, right) => (left << 2) ^ right?.GetHashCode() ?? left);

        /// <summary>
        /// Raises the empty event.
        /// </summary>
        /// <param name="optional"></param>
        /// <param name="onEmpty">The onEmpty.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        public static Optional<T> OnEmpty<T>(this Optional<T> optional, Func<Optional<T>> onEmpty)
            => optional.Any() ? optional : onEmpty() is Optional<T> op ? op : optional;

        /// <summary>
        /// Raises the error event.
        /// </summary>
        /// <param name="optional">The optional.</param>
        /// <param name="onError">The onError.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Optional<T> OnError<T>(this Optional<T> optional, Func<Exception, T> onError)
            => optional._error == default ? optional : Optional.Create(onError(optional._error));

        /// <summary>
        /// Raises the error event.
        /// </summary>
        /// <param name="optional">The optional.</param>
        /// <param name="onError">The onError.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Optional<T> OnError<T>(this Optional<T> optional, Func<Exception, T[]> onError)
           => optional._error == default ? optional : Optional.Create(onError(optional._error));

        /// <summary>
        /// Raises the single event.
        /// </summary>
        /// <param name="optional"></param>
        /// <param name="onSingle">The onSingle.</param>
        /// <returns>The <see cref="T:IEnumerable{T}"/>.</returns>
        public static Optional<T> OnSingle<T>(this Optional<T> optional, Func<T, IEnumerable<T>> onSingle)
            => optional.Skip(1).Any() ? optional : Optional.Create<T>(onSingle(optional.FirstOrDefault()).ToArray());

        /// <summary>
        /// Raises the multi event.
        /// </summary>
        /// <param name="optional"></param>
        /// <param name="onMulti">The onMulti.</param>
        /// <returns>The <see cref="T:T"/>.</returns>
        public static Optional<T> OnMulti<T>(this Optional<T> optional, Func<IEnumerable<T>, T> onMulti)
            => optional.Values.Skip(1).Any() ? Optional.Create<T>(onMulti(optional.Values)) : optional;

        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="optional">The optional.</param>
        /// <returns>The <see cref="T:Optional{T1}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public static Optional<T1> Map<T, T1>(this Optional<T> optional)
            => Optional.Create(optional.Select(x => x is T1 t1 ? t1 : default)
                .Where(x => x != null)
                .ToList());
    }
}
