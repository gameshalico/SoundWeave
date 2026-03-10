#nullable enable

using System.Collections.Generic;

namespace SoundWeave
{
    public sealed class SoundGroupMap<TKey> where TKey : notnull
    {
        private readonly Dictionary<TKey, SoundGroup> _groups;

        public SoundGroupMap(int capacity = 0)
        {
            _groups = new Dictionary<TKey, SoundGroup>(capacity);
        }

        public SoundGroup this[TKey key] => GetOrAdd(key);

        public SoundGroup GetOrAdd(TKey key)
        {
            if (_groups.TryGetValue(key, out var group))
                return group;

            group = new SoundGroup();
            _groups.Add(key, group);
            return group;
        }

        public bool TryGet(TKey key, out SoundGroup group)
        {
            return _groups.TryGetValue(key, out group!);
        }

        public void Remove(TKey key)
        {
            if (!_groups.TryGetValue(key, out var group))
                return;

            group.StopAll();
            _groups.Remove(key);
        }

        public void Clear()
        {
            foreach (var group in _groups.Values)
                group.StopAll();

            _groups.Clear();
        }
    }
}
