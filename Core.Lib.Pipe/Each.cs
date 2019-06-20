using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Pipe
{

    public class Each<TDelegate> : IDelegateEnumerator<TDelegate>
        where TDelegate : Delegate
    {
        private readonly IEnumerator<IDelegateEnumerator<TDelegate>> _enumerator;

        protected internal Each(IEnumerator<IDelegateEnumerator<TDelegate>> enumerator)
        {
            _enumerator = enumerator;
        }

        public TDelegate Current { get; private set; }
        object IEnumerator.Current => Current;
        public bool MoveNext()
           => (_enumerator.Current?.MoveNext() ?? false)
               ? AssignCurrent()
               : _enumerator.MoveNext()
                   && MoveNext();

        private bool AssignCurrent()
        {
            Current = _enumerator.Current.Current;
            return true;
        }

        public void Reset()
        {
            _enumerator.Reset();
            Current = default;
        }
        public void Dispose()
        {
            _enumerator.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
