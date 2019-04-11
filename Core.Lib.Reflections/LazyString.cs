
using System;

namespace Core.Lib
{
    /// <summary>
    /// �N�n�Q�B�z�����PB󻳲z�����A�ȳ���Ȧs���n쭨or��ɤ~�i�B泲z��Struct
    /// </summary>
    public readonly struct LazyString
    {
        /// <summary>
        /// The to string (readonly).
        /// </summary>
        private readonly Func<object, object[], string> _toString;

        /// <summary>
        /// The obj (readonly).
        /// </summary>
        private readonly object _obj;

        /// <summary>
        /// The args (readonly).
        /// </summary>
        private readonly object[] _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyString"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="toString">The toString.</param>
        /// <param name="args">The args.</param>
        public LazyString(object obj, Func<object, object[], string> toString, params object[] args)
        {
            _toString = toString;
            _obj = obj;
            _args = args;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString() => _toString?.Invoke(_obj ?? new object(), _args ?? Array.Empty<object>()) ?? "";
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static explicit operator string(LazyString str) => str.ToString();
    }
}