#nullable enable

namespace SoundWeave
{
    public interface ISoundPlayer
    {
        SoundHandle Play(in SoundData data);
    }
}
