#nullable enable

using System;
using System.Collections.Generic;

namespace SoundWeave
{
    public sealed class SoundGroup
    {
        private readonly List<SoundHandle> _handles;

        public SoundGroup(int capacity = 0)
        {
            _handles = new List<SoundHandle>(capacity);
        }

        public int Count
        {
            get
            {
                Prune();
                return _handles.Count;
            }
        }

        public void Add(SoundHandle handle)
        {
            if (!handle.IsActive())
                throw new InvalidOperationException("SoundHandle is not active.");

            _handles.Add(handle);
        }

        public bool Remove(SoundHandle handle)
        {
            return _handles.Remove(handle);
        }

        public void StopAll()
        {
            for (var i = _handles.Count - 1; i >= 0; i--)
            {
                var handle = _handles[i];
                if (handle.IsActive())
                    handle.Stop();
            }

            _handles.Clear();
        }

        public void Clear()
        {
            _handles.Clear();
        }

        public Enumerator GetEnumerator()
        {
            Prune();
            return new Enumerator(_handles);
        }

        private void Prune()
        {
            for (var i = _handles.Count - 1; i >= 0; i--)
            {
                if (!_handles[i].IsActive())
                    _handles.RemoveAt(i);
            }
        }

        public struct Enumerator : IDisposable
        {
            private readonly List<SoundHandle> _list;
            private readonly int _count;
            private int _index;

            internal Enumerator(List<SoundHandle> list)
            {
                _list = list;
                _count = list.Count;
                _index = -1;
            }

            public SoundHandle Current => _list[_index];

            public bool MoveNext()
            {
                _index++;
                return _index < _count;
            }

            public void Dispose()
            {
            }
        }
    }
}
