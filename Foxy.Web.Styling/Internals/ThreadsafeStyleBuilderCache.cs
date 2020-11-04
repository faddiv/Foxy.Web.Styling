using System;
using System.Collections.Concurrent;

namespace Blazorify.Utilities.Styling.Internals
{
    internal class ThreadsafeStyleBuilderCache
    {
        private readonly ConcurrentDictionary<Type, ProcessStyleDelegate> _styleExtractors = new ConcurrentDictionary<Type, ProcessStyleDelegate>();

        public ProcessStyleDelegate GetOrAdd(Type type, Func<Type, ProcessStyleDelegate> create)
        {
            return _styleExtractors.GetOrAdd(type, create);
        }

        public void ClearCache()
        {
            _styleExtractors.Clear();
        }
    }
}
