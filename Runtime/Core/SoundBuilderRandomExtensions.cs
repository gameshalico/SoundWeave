#nullable enable

using UnityEngine;

namespace SoundWeave
{
    public static class SoundBuilderRandomExtensions
    {
        public static SoundBuilder WithRandomVolume(this SoundBuilder builder, float min, float max)
        {
            return builder.WithVolume(Random.Range(min, max));
        }

        public static SoundBuilder WithRandomPitch(this SoundBuilder builder, float min, float max)
        {
            return builder.WithPitch(Random.Range(min, max));
        }
    }
}
