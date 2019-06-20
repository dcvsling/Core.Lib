using System;
using System.Collections;

namespace Core.Lib.Pipe
{
    internal class MapFromPipeBuildler<TParentDelegate, TDelegate> : IDelegateEnumerator<TDelegate>
        where TDelegate : Delegate
        where TParentDelegate : Delegate
    {
        private readonly Func<TParentDelegate, TDelegate> _map;
        private readonly IDelegateEnumerator<TParentDelegate> _last;

        public TDelegate Current { get; private set; }
        object IEnumerator.Current => Current;

        protected internal MapFromPipeBuildler(IDelegateEnumerator<TParentDelegate> last, Func<TParentDelegate, TDelegate> map)
        {
            _map = map;
            _last = last;
        }

        public bool MoveNext()
        {
            var result = _last.MoveNext();
            Current = _map(_last.Current);
            return result;
        }
        public void Reset()
        {
            _last.Reset();
            Current = default;
        }
        public void Dispose()
        {
            _last.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
