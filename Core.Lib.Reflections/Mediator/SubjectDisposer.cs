using System;

namespace Core.Lib.Reflections.Observables
{
    internal sealed class SubjectDisposer : IDisposable
    {
        private readonly Action _action;

        public SubjectDisposer(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
            GC.SuppressFinalize(this);
        }
    }
}
