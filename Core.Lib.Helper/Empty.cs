using System;
using System.Threading.Tasks;

namespace Core.Lib.Reflections
{
    /// <summary>
    /// The empty class.
    /// </summary>
    public class Empty
    {
        public const string String = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="Empty"/> class.
        /// </summary>
        public Empty() { }

        /// <summary>
        /// Gets the default.
        /// </summary>
        public static Empty Default => new Empty();

        /// <summary>
        /// Gets the action.
        /// </summary>
        public static Action Action => () => { };

        /// <summary>
        /// The config.
        /// </summary>
        /// <returns>The <see cref="T:Action{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Action<T> Config<T>() => _ => { };

        /// <summary>
        /// The config async.
        /// </summary>
        /// <returns>The <see cref="T:Func{T,Task}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Func<T, Task> ConfigAsync<T>()
            => _ => Task.CompletedTask;
    }
}
