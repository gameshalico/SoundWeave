#nullable enable

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave
{
    public readonly struct SoundBuilder : IDisposable
    {
        private readonly ushort _version;
        private readonly SoundBuilderBuffer _buffer;

        public static SoundBuilder Create()
        {
            return new SoundBuilder(SoundBuilderBuffer.Rent());
        }

        internal SoundBuilder(SoundBuilderBuffer buffer)
        {
            _version = buffer.Version;
            _buffer = buffer;
        }

        internal SoundBuilder WithAllParams(
            Vector3 position,
            AudioClip? clip,
            AudioMixerGroup? outputAudioMixerGroup = null,
            bool mute = false,
            float volume = 1f,
            float pitch = 1f,
            int priority = 128,
            float panStereo = 0f,
            int startSample = 0,
            bool loop = false,
            TimingMode timingMode = TimingMode.Immediate,
            double timingValue = 0d,
            double? scheduledEndTime = null)
        {
            ThrowIfDisposed();
            _buffer.Position = position;
            _buffer.Clip = clip;
            _buffer.OutputAudioMixerGroup = outputAudioMixerGroup;
            _buffer.Mute = mute;
            _buffer.Volume = volume;
            _buffer.Pitch = pitch;
            _buffer.Priority = priority;
            _buffer.PanStereo = panStereo;
            _buffer.StartSample = startSample;
            _buffer.Loop = loop;
            _buffer.TimingMode = timingMode;
            _buffer.TimingValue = timingValue;
            _buffer.ScheduledEndTime = scheduledEndTime;
            return this;
        }

        public AudioClip? Clip
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Clip;
            }
        }

        public AudioMixerGroup? OutputAudioMixerGroup
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.OutputAudioMixerGroup;
            }
        }

        public bool Mute
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Mute;
            }
        }

        public float Volume
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Volume;
            }
        }

        public float Pitch
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Pitch;
            }
        }

        public int Priority
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Priority;
            }
        }

        public float PanStereo
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.PanStereo;
            }
        }

        public int StartSample
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.StartSample;
            }
        }

        public bool Loop
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.Loop;
            }
        }

        public TimingMode TimingMode
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.TimingMode;
            }
        }

        public double TimingValue
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.TimingValue;
            }
        }

        public double? ScheduledEndTime
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.ScheduledEndTime;
            }
        }

        public double PlayDspTime
        {
            get
            {
                ThrowIfDisposed();
                return SoundUtility.EvaluateDspTime(_buffer.TimingMode, _buffer.TimingValue);
            }
        }

        public SoundBuilder WithClip(AudioClip clip)
        {
            ThrowIfDisposed();
            _buffer.Clip = clip;
            return this;
        }

        public SoundBuilder WithPosition(Vector3 position)
        {
            ThrowIfDisposed();
            _buffer.Position = position;
            return this;
        }

        public SoundBuilder WithOutputAudioMixerGroup(AudioMixerGroup group)
        {
            ThrowIfDisposed();
            _buffer.OutputAudioMixerGroup = group;
            return this;
        }

        public SoundBuilder WithMute(bool mute)
        {
            ThrowIfDisposed();
            _buffer.Mute = mute;
            return this;
        }

        public SoundBuilder WithVolume(float volume)
        {
            ThrowIfDisposed();
            _buffer.Volume = volume;
            return this;
        }

        public SoundBuilder WithPitch(float pitch)
        {
            ThrowIfDisposed();
            _buffer.Pitch = pitch;
            return this;
        }

        public SoundBuilder WithPriority(int priority)
        {
            ThrowIfDisposed();
            _buffer.Priority = priority;
            return this;
        }

        public SoundBuilder WithPanStereo(float panStereo)
        {
            ThrowIfDisposed();
            _buffer.PanStereo = panStereo;
            return this;
        }

        public SoundBuilder WithStartSample(int startSample)
        {
            ThrowIfDisposed();
            _buffer.StartSample = startSample;
            return this;
        }

        public SoundBuilder WithLoop(bool loop)
        {
            ThrowIfDisposed();
            _buffer.Loop = loop;
            return this;
        }

        public SoundBuilder WithImmediate()
        {
            ThrowIfDisposed();
            _buffer.TimingMode = TimingMode.Immediate;
            return this;
        }

        public SoundBuilder WithDelay(double delay)
        {
            ThrowIfDisposed();
            _buffer.TimingMode = TimingMode.Delay;
            _buffer.TimingValue = delay;
            return this;
        }

        public SoundBuilder WithSchedule(double dspTime)
        {
            ThrowIfDisposed();
            _buffer.TimingMode = TimingMode.Schedule;
            _buffer.TimingValue = dspTime;
            return this;
        }

        public SoundBuilder WithScheduledEndTime(double scheduledEndTime)
        {
            ThrowIfDisposed();
            _buffer.ScheduledEndTime = scheduledEndTime;
            return this;
        }

        internal SoundData Build()
        {
            ThrowIfDisposed();

            if (_buffer.Clip == null)
                throw new InvalidOperationException("The AudioClip is null.");

            var data = new SoundData(
                _buffer.Clip,
                _buffer.Position,
                _buffer.OutputAudioMixerGroup,
                _buffer.Mute,
                _buffer.Volume,
                _buffer.Pitch,
                _buffer.Priority,
                _buffer.PanStereo,
                _buffer.StartSample,
                _buffer.Loop,
                _buffer.TimingMode,
                _buffer.TimingValue,
                _buffer.ScheduledEndTime);

            Dispose();
            return data;
        }

        public void Dispose()
        {
            if (_buffer == null)
                return;
            if (_buffer.Version != _version)
                return;

            SoundBuilderBuffer.Return(_buffer);
        }

        private void ThrowIfDisposed()
        {
            if (_buffer == null || _buffer.Version != _version)
                throw new ObjectDisposedException(nameof(SoundBuilder),
                    "This SoundBuilder has already been consumed or disposed.");
        }
    }
}
