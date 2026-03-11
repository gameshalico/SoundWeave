#nullable enable

using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Tests
{
    internal sealed class MockSoundControl : ISoundControl
    {
        public ushort Version { get; set; }
        public PlaybackState State { get; set; }
        public double PlayDspTime { get; set; }
        public Vector3 Position { get; set; }
        public IAudioGenerator AudioGenerator { get; set; } = null!;
        public AudioMixerGroup? OutputAudioMixerGroup { get; set; }
        public bool IsPlaying { get; set; }
        public float Time { get; set; }
        public bool Mute { get; set; }
        public float Volume { get; set; } = 1f;
        public float Pitch { get; set; } = 1f;
        public int Priority { get; set; } = 128;
        public float PanStereo { get; set; }
        public int TimeSamples { get; set; }
        public bool Loop { get; set; }

        public bool StopCalled { get; private set; }
        public bool PauseCalled { get; private set; }
        public bool UnPauseCalled { get; private set; }
        public double? ScheduledStartTime { get; private set; }
        public double? ScheduledEndTime { get; private set; }

        public void Stop() => StopCalled = true;
        public void Pause() => PauseCalled = true;
        public void UnPause() => UnPauseCalled = true;
        public void SetScheduledStartTime(double time) => ScheduledStartTime = time;
        public void SetScheduledEndTime(double time) => ScheduledEndTime = time;

        public void Invalidate() => Version++;
    }
}
