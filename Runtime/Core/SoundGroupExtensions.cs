#nullable enable

using UnityEngine;

namespace SoundWeave
{
    public static class SoundGroupExtensions
    {
        public static SoundHandle AddToGroup(this SoundHandle handle, SoundGroup group)
        {
            group.Add(handle);
            return handle;
        }

        public static SoundHandle AddToGroup<TKey>(
            this SoundHandle handle, SoundGroupMap<TKey> map, TKey key)
            where TKey : notnull
        {
            map[key].Add(handle);
            return handle;
        }

        public static SoundBuilder AdjustVolumeForOverlap(this SoundBuilder builder, SoundGroup group)
        {
            var dspTime = builder.PlayDspTime;
            var volumeRate = 1f;

            foreach (var handle in group)
            {
                if (!handle.IsActive() || handle.Volume <= 0)
                    continue;

                var diff = Mathf.Abs((float)(dspTime - handle.PlayDspTime));

                if (diff < 0.025f)
                {
                    volumeRate = 0;
                    break;
                }

                if (diff < 0.05f)
                    volumeRate *= 0.8f;
                else if (diff < 0.1f)
                    volumeRate *= 0.9f;
            }

            return builder.WithVolume(builder.Volume * volumeRate);
        }

        public static SoundBuilder AdjustVolumeForOverlap<TKey>(
            this SoundBuilder builder, SoundGroupMap<TKey> map, TKey key)
            where TKey : notnull
        {
            return AdjustVolumeForOverlap(builder, map[key]);
        }
    }
}
