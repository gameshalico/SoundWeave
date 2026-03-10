#nullable enable

using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave
{
    public interface ISoundControl
    {
        ushort Version { get; }
        PlaybackState State { get; }
        double PlayDspTime { get; }

        Vector3 Position { get; set; }
        AudioClip Clip { get; set; }
        AudioMixerGroup OutputAudioMixerGroup { get; set; }
        bool IsPlaying { get; }
        float Time { get; set; }
        bool Mute { get; set; }
        float Volume { get; set; }
        float Pitch { get; set; }
        int Priority { get; set; }
        float PanStereo { get; set; }
        int TimeSamples { get; set; }
        bool Loop { get; set; }

        void Stop();
        void Pause();
        void UnPause();
        void SetScheduledStartTime(double time);
        void SetScheduledEndTime(double time);
    }
}
