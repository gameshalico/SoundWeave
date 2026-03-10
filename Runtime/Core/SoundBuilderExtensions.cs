#nullable enable

namespace SoundWeave
{
    public static class SoundBuilderExtensions
    {
        public static SoundHandle Play(this SoundBuilder builder, ISoundPlayer player)
        {
            return player.Play(builder.Build());
        }
    }
}
