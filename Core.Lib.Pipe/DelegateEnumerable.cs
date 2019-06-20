using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Pipe
{
    public class DelegateEnumerable<TDelegate, TEnumerator> : IEnumerable<TDelegate>
        where TDelegate : Delegate
        where TEnumerator : IDelegateEnumerator<TDelegate>
    {
        private readonly Func<TDelegate, TEnumerator> _input;
        private readonly IEnumerator<TDelegate> _enumerator;

        public DelegateEnumerable(
            IEnumerator<TDelegate> enumerator,
            Func<TDelegate, TEnumerator> input = default)
        {
            _input = input;
            _enumerator = enumerator;
        }

        public IEnumerator<TDelegate> GetEnumerator()
            => (TEnumerator)Activator.CreateInstance(typeof(TEnumerator), _enumerator);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
