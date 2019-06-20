using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Pipe
{

    internal class ConcatDelegateEnumerator<TDelegate> : IDelegateEnumerator<TDelegate> where TDelegate : Delegate
    {
        private readonly IDelegateEnumerator<TDelegate> _enumerator;
        private readonly IEnumerable<TDelegate> _delegates;


        internal protected ConcatDelegateEnumerator(IDelegateEnumerator<TDelegate> enumerator, IEnumerable<TDelegate> delegates)
        {
            _enumerator = enumerator;
            _delegates = delegates;
        }

        public TDelegate Current { get; private set; }
        object IEnumerator.Current => Current;


        public bool MoveNext()
            => Move();

        private Func<bool> Move { get; set; }

        private bool MoveAndGetNew()
        {
            foreach (var d in _delegates)
            {
                Current = d;
                return true;
            }
            return false;
        }

        private bool MoveAndGetOld()
        {
            var result = _enumerator.MoveNext();
            if (!result)
            {
                Move = MoveAndGetNew;
                return Move();
            }
            Current = _enumerator.Current;
            return result;
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
