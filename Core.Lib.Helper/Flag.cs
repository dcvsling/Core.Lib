using System;
using System.Runtime.CompilerServices;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The flag class.
    /// </summary>
    /// <typeparam name="TFlag"></typeparam>
    public abstract class Flag<TFlag> : Flag<TFlag, int> where TFlag : Flag<TFlag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flag{TFlag}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected Flag(int value, string name) : base(value, name)
        {
        }
    }

    /// <summary>
    /// The flag class.
    /// </summary>
    /// <typeparam name="TFlag"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class Flag<TFlag,TValue>
        where TFlag : Flag<TFlag, TValue> 
    {
        /// <summary>
        /// The name (readonly).
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Flag{TFlag, TValue}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected Flag(TValue value, string name)
        {
            Value = value;
            _name = name;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString() => _name;

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="T:TFlag"/>.</returns>
        public static TFlag Create(TValue value, [CallerMemberName] string name = default)
            => (TFlag)Activator.CreateInstance(typeof(TFlag), value, name);

        /// <summary>
        /// Gets the value.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj) => Value.GetHashCode() == obj.GetHashCode();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public static implicit operator TValue(Flag<TFlag,TValue> flag)
            => flag.Value;

        /// <summary>
        /// The operator ==.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool operator ==(Flag<TFlag, TValue> flag, TValue value)
            => flag.GetHashCode() == value.GetHashCode();

        /// <summary>
        /// The operator !=.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool operator !=(Flag<TFlag, TValue> flag, TValue value)
            => !(flag == value);
    }
}
