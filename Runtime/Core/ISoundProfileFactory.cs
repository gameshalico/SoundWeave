#nullable enable

namespace SoundWeave
{
    public interface ISoundProfileFactory
    {
        SoundBuilder CreateBuilder();
        bool IsValid();
    }
}
