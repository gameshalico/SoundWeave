#nullable enable

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave
{
    public readonly struct SoundHandle : IEquatable<SoundHandle>
    {
        private readonly ISoundControl? _control;
        private readonly ushort _version;

        public SoundHandle(ISoundControl control)
        {
            _version = control.Version;
            _control = control;
        }

        public static SoundHandle Invalid => default;

        public Vector3 Position
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Position;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Position = value;
            }
        }

        public AudioClip Clip
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Clip;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Clip = value;
            }
        }

        public AudioMixerGroup? OutputAudioMixerGroup
        {
            get
            {
                ThrowIfInvalid();
                return _control!.OutputAudioMixerGroup;
            }
            set
            {
                ThrowIfInvalid();
                _control!.OutputAudioMixerGroup = value;
            }
        }

        public bool IsPlaying
        {
            get
            {
                ThrowIfInvalid();
                return _control!.IsPlaying;
            }
        }

        public float Time
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Time;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Time = value;
            }
        }

        public bool Mute
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Mute;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Mute = value;
            }
        }

        public float Volume
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Volume;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Volume = value;
            }
        }

        public float Pitch
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Pitch;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Pitch = value;
            }
        }

        public int Priority
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Priority;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Priority = value;
            }
        }

        public float PanStereo
        {
            get
            {
                ThrowIfInvalid();
                return _control!.PanStereo;
            }
            set
            {
                ThrowIfInvalid();
                _control!.PanStereo = value;
            }
        }

        public int TimeSamples
        {
            get
            {
                ThrowIfInvalid();
                return _control!.TimeSamples;
            }
            set
            {
                ThrowIfInvalid();
                _control!.TimeSamples = value;
            }
        }

        public bool Loop
        {
            get
            {
                ThrowIfInvalid();
                return _control!.Loop;
            }
            set
            {
                ThrowIfInvalid();
                _control!.Loop = value;
            }
        }

        public double PlayDspTime
        {
            get
            {
                ThrowIfInvalid();
                return _control!.PlayDspTime;
            }
        }

        public PlaybackState State
        {
            get
            {
                ThrowIfInvalid();
                return _control!.State;
            }
        }

        public bool IsActive()
        {
            return _control != null && _control.Version == _version;
        }

        public void Stop()
        {
            ThrowIfInvalid();
            _control!.Stop();
        }

        public void Pause()
        {
            ThrowIfInvalid();
            _control!.Pause();
        }

        public void UnPause()
        {
            ThrowIfInvalid();
            _control!.UnPause();
        }

        public void SetScheduledStartTime(double time)
        {
            ThrowIfInvalid();
            _control!.SetScheduledStartTime(time);
        }

        public void SetScheduledEndTime(double time)
        {
            ThrowIfInvalid();
            _control!.SetScheduledEndTime(time);
        }

        private void ThrowIfInvalid()
        {
            if (_control == null || _control.Version != _version)
                throw new InvalidOperationException("SoundHandle is no longer valid.");
        }

        public bool Equals(SoundHandle other)
        {
            return _control == other._control && _version == other._version;
        }

        public override bool Equals(object? obj)
        {
            return obj is SoundHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_control, _version);
        }

        public static bool operator ==(SoundHandle left, SoundHandle right) => left.Equals(right);
        public static bool operator !=(SoundHandle left, SoundHandle right) => !left.Equals(right);
    }
}
