using Core.Lib.Reflections;
using System;
using System.Collections.Concurrent;

namespace Core.Lib
{
    public static class Accessor
    {
        private static ConcurrentDictionary<Type, (ConcurrentDictionary<string, Func<object, object>> gets, ConcurrentDictionary<string, Action<object, object>> sets)> _cache
            = new ConcurrentDictionary<Type, (ConcurrentDictionary<string, Func<object, object>> gets, ConcurrentDictionary<string, Action<object, object>> sets)>();

        public static IAccessor Get(Type type)
            => new DefaultAccessor(
                type,
                _cache.GetOrAdd(
                    type,
                    _ => (
                        gets: new ConcurrentDictionary<string, Func<object, object>>(),
                        sets: new ConcurrentDictionary<string, Action<object, object>>()
                    )));
    }
}
