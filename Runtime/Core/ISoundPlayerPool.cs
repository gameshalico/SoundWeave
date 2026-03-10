#nullable enable

using System;

namespace SoundWeave
{
    public interface ISoundPlayerPool : ISoundPlayer, IDisposable
    {
        int ActiveCount { get; }
        int FreeCount { get; }
    }
}
