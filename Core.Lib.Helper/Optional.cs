using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The optional class.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// The optional factory (readonly). Value: new ConcurrentDictionary&lt;Type, Func&lt;object[], object&gt;&gt;().
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<object[], object>> _optionalFactory
            = new ConcurrentDictionary<Type, Func<object[], object>>();

        /// <summary>
        /// Create the optional factory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="T:Func{object[], object}"/>.</returns>
        private static Func<object[], object> CreateOptionalFactory(Type type)
        {
            var ctor = typeof(Optional<>).MakeGenericType(type).GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .First(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(IEnumerable<>).MakeGenericType(type));
            var input = Expression.Parameter(typeof(object[]));
            var data = Expression.Call(typeof(Enumerable).GetMethod(nameof(Enumerable.OfType)).MakeGenericMethod(type), input);
            var create = Expression.New(ctor, new[] { data });
            var toObj = Expression.TypeAs(create, typeof(object));
            return Expression.Lambda<Func<object[], object>>(toObj, input).Compile();
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="ts">The ts.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object Create(Type type, params object[] ts)
            => _optionalFactory.GetOrAdd(type, CreateOptionalFactory)
                .Invoke(ts);

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="ts">The ts.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Optional<T> Create<T>(params T[] ts)
            => new Optional<T>(ts);

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="ts">The ts.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Optional<T> Create<T>(List<T> ts)
            => new Optional<T>(ts);

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static Optional<T> Error<T>(Exception ex)
            => new Optional<T>(ex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    public readonly struct Optional<T> :
        IEnumerable<T>,
        IEqualityComparer<T>,
        IEqualityComparer<IEnumerable<T>>,
        IEqualityComparer<Optional<T>>
    {
        /// <summary>
        /// The hash seed (const). Value: 390.
        /// </summary>
        private const int _hashSeed = 390;

        /// <summary>
        /// The error (readonly).
        /// </summary>
        internal readonly Exception _error;

        /// <summary>
        /// The equality comparer (readonly).
        /// </summary>
        private readonly OptionalEqualityComparer _equalityComparer;

        /// <summary>
        /// The hash code (readonly).
        /// </summary>
        internal readonly int _hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Optional"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        internal Optional(IEnumerable<T> target)
        {
            Value = target.FirstOrDefault();
            Values = target;
            _error = default;
            _hashCode = Values.CreateHashCode(_hashSeed);
            _equalityComparer = new OptionalEqualityComparer(_hashSeed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Optional"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal Optional(Exception exception)
        {
            Value = default;
            Values = Enumerable.Empty<T>();
            _error = exception;
            _hashCode = Values.CreateHashCode(_hashSeed);
            _equalityComparer = new OptionalEqualityComparer(_hashSeed);
        }

        /// <summary>
        /// The value (readonly).
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// The values (readonly).
        /// </summary>
        public readonly IEnumerable<T> Values;

        /// <summary>
        /// Raises the error event.
        /// </summary>
        /// <param name="onError">The onError.</param>
        /// <returns>The <see cref="T:Optional{T}"/>.</returns>
        /// <typeparam name="TException"></typeparam>
        public Optional<T> OnError<TException>(Func<Exception, TException> onError)
            where TException : Exception
            => ReferenceEquals(_error, default) ? this : throw onError(_error) ?? _error;

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _hashCode;

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
            => JsonConvert.SerializeObject(
                Values,
                new JsonSerializerSettings {
                    Formatting = Formatting.Indented,
                    DateFormatString = "yyyy/MM/dd hh:mm:ss",
                });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        public static implicit operator T(Optional<T> option) => option.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        public static implicit operator T[](Optional<T> option) => option.Values.ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        public static implicit operator List<T>(Optional<T> option) => option.Values.ToList();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="t"></param>
        //public static implicit operator Optional<T>(T t) => Optional.Create<T>(t);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        public static implicit operator Optional<T>(T[] ts) => Optional.Create<T>(ts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tlist"></param>
        public static implicit operator Optional<T>(List<T> tlist) => Optional.Create<T>(tlist);

        /// <summary>
        /// The operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool operator ==(Optional<T> left, Optional<T> right) => left.Equals(right);

        /// <summary>
        /// The operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool operator !=(Optional<T> left, Optional<T> right) => !(left == right);

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>The <see cref="T:IEnumerator{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(T x, T y) => _equalityComparer.Equals(x, y);

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(T obj) => _equalityComparer.GetHashCode(obj);

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y) => _equalityComparer.Equals(x, y);

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(IEnumerable<T> obj) => _equalityComparer.GetHashCode(obj);

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(Optional<T> x, Optional<T> y) => _equalityComparer.Equals(x, y);

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(Optional<T> obj) => _equalityComparer.GetHashCode(obj);

        /// <summary>
        /// The optional equality comparer class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class OptionalEqualityComparer
                : IEqualityComparer<T>,
                  IEqualityComparer<IEnumerable<T>>,
                  IEqualityComparer<Optional<T>>
        {
            /// <summary>
            /// The hash seed (readonly).
            /// </summary>
            private readonly int _hashSeed;

            /// <summary>
            /// Initializes a new instance of the <see cref="OptionalEqualityComparer{T}"/> class.
            /// </summary>
            /// <param name="hashSeed">The hashSeed.</param>
            internal OptionalEqualityComparer(int hashSeed)
            {
                _hashSeed = hashSeed;
            }

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public bool Equals(T x, T y)
                => x?.GetHashCode() == y?.GetHashCode();

            /// <summary>
            /// Get the hash code.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>The <see cref="int"/>.</returns>
            public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public bool Equals(IEnumerable<T> x, IEnumerable<T> y) => GetHashCode(x) == GetHashCode(y);

            /// <summary>
            /// Get the hash code.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>The <see cref="int"/>.</returns>
            public int GetHashCode(IEnumerable<T> obj) => obj?.CreateHashCode(_hashSeed) ?? 0;

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public bool Equals(Optional<T> x, Optional<T> y) => GetHashCode(x) == GetHashCode(y);

            /// <summary>
            /// Get the hash code.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>The <see cref="int"/>.</returns>
            public int GetHashCode(Optional<T> obj) => obj.GetHashCode();
        }
    }
}
