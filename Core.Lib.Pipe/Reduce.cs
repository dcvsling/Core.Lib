using Core.Lib.Reflections.Executors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Pipe
{
    internal class Reduce<TDelegate> : IDelegateEnumerator<TDelegate>
        where TDelegate : Delegate
    {
        private Func<object[], Func<TDelegate, TDelegate, object>> _reduce;
        private readonly IEnumerator<TDelegate> _enumerator;
        private TDelegate _last;
        public Reduce(IEnumerator<TDelegate> enumerator)
        {
            _enumerator = enumerator;
            _reduce = args => (left, right) => ReduceIterator(args, left, right);
        }

        public bool MoveNext()
        {
            _last = _enumerator.Current;
            var result = _enumerator.MoveNext();
            Current = result
                ? DelegateExecutorHelper.Wrap<TDelegate>(args => _reduce.Invoke(args).Invoke(_last, _enumerator.Current))
                : default;
            return result;
        }

        public Func<IEnumerable<TDelegate>, TDelegate> Invoker
            => delegates => DelegateExecutorHelper.Wrap<TDelegate>(
                args => _reduce.Invoke(args)
                    .Invoke(delegates.First(), delegates.Skip(1).First()));

        public TDelegate Current { get; private set; }
        object IEnumerator.Current => Current;

        private static IEnumerable<object> ReduceIterator(object[] args, TDelegate left, TDelegate right)
        {
            yield return left.Invoke(args);
            yield return right.Invoke(args);
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