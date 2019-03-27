using System;

namespace Core.Lib.Reflections
{
    /// <summary>
    /// The IProperties interface.
    /// </summary>

    public interface IProperties : IDisposable
    {
        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="T:T"/>.</returns>
        /// <typeparam name="T"></typeparam>
        T Get<T>(string name = "");

        /// <summary>
        /// Set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(T value, string name = "");
    }
}
