using System;
using System.Collections.Generic;

namespace Siege.Gameplay
{
    public sealed class TempList<T> : List<T>, IDisposable
    {
        const int MaxPoolSize = 16;

        static readonly Stack<TempList<T>> _pool = new();

        TempList() { }

        public static TempList<T> Get()
        {
            if (_pool.TryPop(out var list))
            {
                list.Clear();
                return list;
            }

            return new TempList<T>();
        }

        public void Dispose()
        {
            Clear();
            if (_pool.Count < MaxPoolSize)
                _pool.Push(this);
        }
    }
}
