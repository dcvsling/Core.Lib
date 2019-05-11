using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Lib.Reflections.Properties
{
    /// <summary>
    /// The default properties class.
    /// </summary>
    [DebuggerDisplay("{DebugView}")]
    [DataContract]
    internal sealed class DefaultProperties : IProperties
    {
        /// <summary>
        /// The properties (readonly).
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _properties;

        /// <summary>
        /// Gets the debug view.
        /// </summary>
        [DataMember]
        internal IDictionary<string, object> DebugView => _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProperties"/> class.
        /// </summary>
        public DefaultProperties()
        {
            _properties = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="T:T"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public T Get<T>(string name = Empty.String)
            => _properties.TryGetValue(name, out var value) ? (T)value : default;

        /// <summary>
        /// Set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(T value, string name = Empty.String)
            => _properties.AddOrUpdate(name, value, (_, __) => value);

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _properties.Values.OfType<IDisposable>().Each(x => x.Dispose());
            _properties.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
