using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Lib.Reflections
{
    /// <summary>
    /// The linq helper class.
    /// </summary>
    [DebuggerStepThrough]
    public static class LinqHelper
    {
        /// <summary>
        /// https://github.com/dotnet/corefx/issues/2075
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public static IEnumerable<T> Emit<T>(this T t)
        {
            yield return t;
        }

        /// <summary>
        /// https://github.com/dotnet/corefx/issues/2075
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seq"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public static IEnumerable<T> Append<T>(this IEnumerable<T> seq, T item)
        {
            foreach (var t in seq)
            {
                yield return t;
            }
            yield return item;
        }

        /// <summary>
        /// https://github.com/dotnet/corefx/issues/2075
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seq"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> seq, T item)
        {
            yield return item;
            foreach (var t in seq)
            {
                yield return t;
            }
        }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="T:T"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static T Configure<T>(this T options, Action<T> config)
        {
            config?.Invoke(options);
            return options;
        }

        /// <summary>
        /// The aggregate seed.
        /// </summary>
        /// <param name="seq">The seq.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="T:TSeed"/>.</returns>
        /// <typeparam name="TSeed"></typeparam>
        /// <typeparam name="T"></typeparam>
        [DebuggerHidden]
        public static TSeed AggregateSeed<TSeed, T>(this IEnumerable<T> seq, TSeed seed, Action<TSeed, T> action)
            => seq.Aggregate(seed, (s, next) =>
            {
                action(s, next);
                return s;
            });

        /// <summary>
        /// The each.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="action">The action.</param>
        /// <typeparam name="T"></typeparam>
        /// <summary>
        /// The each.
        /// </summary>
        [DebuggerHidden]
        public static void Each<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var t in sequence)
            {
                action(t);
            }
        }

        /// <summary>
        /// The each async.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="task">The task.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <typeparam name="T"></typeparam>
        [DebuggerHidden]
        public static Task EachAsync<T>(this IEnumerable<T> sequence, Func<T, Task> task)
            => Task.WhenAll(sequence.Select(task));

        /// <summary>
        /// Do.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="T:IEnumerable{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        [DebuggerHidden]
        public static IEnumerable<T> Do<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var t in sequence)
            {
                action(t);
                yield return t;
            }
        }


        /// <summary>
        /// The recurse.
        /// </summary>
        /// <param name="seq">The seq.</param>
        /// <param name="recurse">The recurse.</param>
        /// <returns>The <see cref="T:TResult"/>.</returns>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        [DebuggerHidden]
        internal static TResult Recurse<T, TResult>(
            this T seq,
            Recursive<Func<T, TResult>> recurse)
            => recurse(recurse)(seq);

        /// <summary>
        /// The recurse.
        /// </summary>
        /// <param name="seq">The seq.</param>
        /// <param name="func">The func.</param>
        /// <returns>The <see cref="T:TResult"/>.</returns>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        [DebuggerHidden]
        public static TResult Recurse<T, TResult>(
            this T seq,
            Func<Func<T, TResult>, Func<T, TResult>> func)
            => seq.Recurse(CreateRecurse(func));

        /// <summary>
        /// Create the recurse.
        /// https://blogs.msdn.microsoft.com/wesdyer/2007/02/02/anonymous-recursion-in-c/
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns>The <see cref="T:R{Func{TA, TR}}"/>.</returns>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TR"></typeparam>
        [DebuggerHidden]
        private static Recursive<Func<TA, TR>> CreateRecurse<TA, TR>(Func<Func<TA, TR>, Func<TA, TR>> f)
            => r => a => f(r(r))(a);
    }
}
