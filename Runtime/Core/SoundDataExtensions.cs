#nullable enable

using UnityEngine;

namespace SoundWeave
{
    public static class SoundDataExtensions
    {
        public static SoundHandle Play(in this SoundData data, ISoundPlayer player)
        {
            return player.Play(data);
        }

        public static SoundBuilder ToBuilder(this SoundData data)
        {
            return SoundBuilder.Create().WithAllParams(
                data.Position, data.Clip, data.OutputAudioMixerGroup,
                data.Mute, data.Volume, data.Pitch, data.Priority,
                data.PanStereo, data.StartSample, data.Loop,
                data.TimingMode, data.TimingValue, data.ScheduledEndTime);
        }
    }
}
