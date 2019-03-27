using System;
using System.Collections.Concurrent;

namespace Core.Lib.Reflections
{


    /// <summary>
    /// The default accessor class.
    /// </summary>
    public class DefaultAccessor : IAccessor
    {
        /// <summary>
        /// The setter (readonly).
        /// </summary>
        private readonly ConcurrentDictionary<string, Action<object, object>> _setter;

        /// <summary>
        /// The getter (readonly).
        /// </summary>
        private readonly ConcurrentDictionary<string, Func<object, object>> _getter;
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccessor"/> class.
        /// </summary>
        /// <param name="getter">The getter.</param>
        /// <param name="setter">The setter.</param>
        public DefaultAccessor(Type type,(ConcurrentDictionary<string,Func<object, object>> getter, ConcurrentDictionary<string, Action<object,object>> setter) accessor)
        {
            _setter = accessor.setter;
            _getter = accessor.getter;
            _type = type;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="property"></param>
        /// <returns>The <see cref="object"/>.</returns>

        public object Get(object target, string name)
            => _getter.GetOrAdd(name,key => ExpressionFactory.CreatePropertyGetter(_type.GetProperty(key)))
                .Invoke(target);


        /// <summary>
        /// Set.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="property"></param>
        /// <param name="value">The value.</param>
        public void Set(object target, string name, object value)
        {
            var type = target.GetType();
            _setter.GetOrAdd(name, key => ExpressionFactory.CreatePropertySetter(_type.GetProperty(key)))
                .Invoke(target, value);
        }
    }
}
