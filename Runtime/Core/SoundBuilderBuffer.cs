#nullable enable

using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave
{
    /// <remarks>メインスレッド専用。マルチスレッドからの Rent/Return は未サポート。</remarks>
    internal sealed class SoundBuilderBuffer
    {
        private const int MaxPoolSize = 64;

        private static readonly SoundBuilderBuffer s_sentinel = new();
        private static SoundBuilderBuffer s_poolHead = s_sentinel;
        private static int s_poolSize;

        private SoundBuilderBuffer? _next;

        public ushort Version { get; private set; }

        public Vector3 Position;
        public IAudioGenerator.Serializable? AudioGenerator;
        public AudioMixerGroup? OutputAudioMixerGroup;
        public bool Mute;
        public float Volume = 1f;
        public float Pitch = 1f;
        public int Priority = 128;
        public float PanStereo;
        public int StartSample;
        public bool Loop;
        public TimingMode TimingMode = TimingMode.Immediate;
        public double TimingValue;
        public double? ScheduledEndTime;

        public static SoundBuilderBuffer Rent()
        {
            if (s_poolHead == s_sentinel)
                return new SoundBuilderBuffer();

            var result = s_poolHead;
            s_poolHead = result._next ?? s_sentinel;
            result._next = null;
            s_poolSize--;
            return result;
        }

        public static void Return(SoundBuilderBuffer buffer)
        {
            buffer.Version++;
            buffer.Reset();

            if (buffer.Version == ushort.MaxValue || s_poolSize >= MaxPoolSize)
                return;

            buffer._next = s_poolHead;
            s_poolHead = buffer;
            s_poolSize++;
        }

        private void Reset()
        {
            Position = Vector3.zero;
            AudioGenerator = null;
            OutputAudioMixerGroup = null;
            Mute = false;
            Volume = 1f;
            Pitch = 1f;
            Priority = 128;
            PanStereo = 0f;
            StartSample = 0;
            Loop = false;
            TimingMode = TimingMode.Immediate;
            TimingValue = 0d;
            ScheduledEndTime = null;
        }
    }
}
