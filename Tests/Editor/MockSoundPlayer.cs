#nullable enable

using System.Collections.Generic;

namespace SoundWeave.Tests
{
    internal sealed class MockSoundPlayer : ISoundPlayer
    {
        private readonly MockSoundControl _control = new();
        public List<SoundData> PlayedData { get; } = new();

        public SoundHandle Play(in SoundData data)
        {
            PlayedData.Add(data);
            _control.Volume = data.Volume;
            _control.Pitch = data.Pitch;
            _control.Loop = data.Loop;
            _control.Mute = data.Mute;
            _control.Priority = data.Priority;
            _control.PanStereo = data.PanStereo;
            _control.Position = data.Position;
            _control.State = PlaybackState.Playing;
            _control.IsPlaying = true;
            return new SoundHandle(_control);
        }

        public MockSoundControl Control => _control;
    }
}
