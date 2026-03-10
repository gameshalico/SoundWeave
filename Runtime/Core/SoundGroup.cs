#nullable enable

using System;
using System.Collections.Generic;

namespace SoundWeave
{
    public sealed class SoundGroup
    {
        private readonly List<SoundHandle> _handles;
        private int _version;

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
            _version++;
        }

        public bool Remove(SoundHandle handle)
        {
            var removed = _handles.Remove(handle);
            if (removed)
                _version++;
            return removed;
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
            _version++;
        }

        public void Clear()
        {
            _handles.Clear();
            _version++;
        }

        public Enumerator GetEnumerator()
        {
            Prune();
            return new Enumerator(this);
        }

        private void Prune()
        {
            var i = 0;
            while (i < _handles.Count)
            {
                if (!_handles[i].IsActive())
                {
                    _handles[i] = _handles[^1];
                    _handles.RemoveAt(_handles.Count - 1);
                }
                else
                {
                    i++;
                }
            }
        }

        public struct Enumerator : IDisposable
        {
            private readonly SoundGroup _group;
            private readonly int _version;
            private readonly int _count;
            private int _index;

            internal Enumerator(SoundGroup group)
            {
                _group = group;
                _version = group._version;
                _count = group._handles.Count;
                _index = -1;
            }

            public SoundHandle Current => _group._handles[_index];

            public bool MoveNext()
            {
                if (_version != _group._version)
                    throw new InvalidOperationException(
                        "SoundGroup was modified during enumeration.");

                _index++;
                return _index < _count;
            }

            public void Dispose()
            {
            }
        }
    }
}
