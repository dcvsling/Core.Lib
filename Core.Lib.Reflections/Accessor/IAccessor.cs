namespace Core.Lib.Reflections
{
    /// <summary>
    /// The IAccessor interface.
    /// </summary>
    public interface IAccessor
    {
        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">property name</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Get(object target, string name);

        /// <summary>
        /// Set.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">property name</param>
        /// <param name="value">The value.</param>
        void Set(object target, string name, object value);
    }
}
