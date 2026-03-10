#nullable enable

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Impl
{
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("")]
    public sealed class WeaveSoundPlayer : MonoBehaviour, ISoundControl, ISoundPlayer
    {
        private AudioSource _audioSource = null!;
        private WeaveSoundPlayerPool? _pool;

        public ushort Version { get; private set; }
        public PlaybackState State { get; private set; }
        public double PlayDspTime { get; private set; }
        public SoundHandle Handle => new(this);
        public bool IsFree => State == PlaybackState.Free;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public AudioClip Clip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }

        public AudioMixerGroup OutputAudioMixerGroup
        {
            get => _audioSource.outputAudioMixerGroup;
            set => _audioSource.outputAudioMixerGroup = value;
        }

        public bool IsPlaying => _audioSource.isPlaying;

        public float Time
        {
            get => _audioSource.time;
            set => _audioSource.time = value;
        }

        public bool Mute
        {
            get => _audioSource.mute;
            set => _audioSource.mute = value;
        }

        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }

        public float Pitch
        {
            get => _audioSource.pitch;
            set => _audioSource.pitch = value;
        }

        public int Priority
        {
            get => _audioSource.priority;
            set => _audioSource.priority = value;
        }

        public float PanStereo
        {
            get => _audioSource.panStereo;
            set => _audioSource.panStereo = value;
        }

        public int TimeSamples
        {
            get => _audioSource.timeSamples;
            set => _audioSource.timeSamples = value;
        }

        public bool Loop
        {
            get => _audioSource.loop;
            set => _audioSource.loop = value;
        }

        public SoundHandle Play(in SoundData data)
        {
            Setup(data);
            PlayAudioSource(data.TimingMode, data.TimingValue);

            if (data.ScheduledEndTime >= 0)
                SetScheduledEndTime(data.ScheduledEndTime);

            return Handle;
        }

        public void Stop()
        {
            State = PlaybackState.Free;
            _audioSource.Stop();
            ReturnToPool();
        }

        public void Pause()
        {
            if (State is PlaybackState.Free or PlaybackState.Paused)
                throw new InvalidOperationException(
                    $"Cannot pause WeaveSoundPlayer in {State} state.");

            _audioSource.Pause();
            State = PlaybackState.Paused;
        }

        public void UnPause()
        {
            if (State != PlaybackState.Paused)
                throw new InvalidOperationException(
                    $"Cannot unpause WeaveSoundPlayer in {State} state.");

            _audioSource.UnPause();
            State = PlaybackState.Playing;
        }

        public void SetScheduledStartTime(double time)
        {
            PlayDspTime = time;
            _audioSource.SetScheduledStartTime(time);
        }

        public void SetScheduledEndTime(double time)
        {
            _audioSource.SetScheduledEndTime(time);
        }

        internal void SetPool(WeaveSoundPlayerPool pool)
        {
            _pool = pool;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            State = PlaybackState.Free;
        }

        private void Update()
        {
            if (State == PlaybackState.Waiting && _audioSource.isPlaying)
                State = PlaybackState.Playing;

            CheckPlayFinished();
        }

        private void OnDestroy()
        {
            _audioSource.Stop();
            Version++;
        }

        private void PlayAudioSource(TimingMode timingMode, double timingValue)
        {
            PlayDspTime = SoundUtility.EvaluateDspTime(timingMode, timingValue);

            switch (timingMode)
            {
                case TimingMode.Immediate:
                    _audioSource.Play();
                    State = PlaybackState.Playing;
                    break;
                case TimingMode.Schedule:
                    _audioSource.PlayScheduled(timingValue);
                    State = PlaybackState.Waiting;
                    break;
                case TimingMode.Delay:
                    _audioSource.PlayDelayed((float)timingValue);
                    State = PlaybackState.Waiting;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timingMode), timingMode, null);
            }
        }

        private void CheckPlayFinished()
        {
            if (State != PlaybackState.Playing)
                return;
            if (_audioSource.loop)
                return;
            if (_audioSource.isPlaying)
                return;

            State = PlaybackState.Free;
            _audioSource.Stop();
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _pool?.ReturnToPool(this);
            Version++;
        }

        private void Setup(in SoundData data)
        {
            if (State != PlaybackState.Free)
            {
                _audioSource.Stop();
                Version++;
            }

            transform.position = data.Position;
            _audioSource.clip = data.Clip;
            _audioSource.loop = data.Loop;
            _audioSource.outputAudioMixerGroup = data.OutputAudioMixerGroup;
            _audioSource.mute = data.Mute;
            _audioSource.volume = data.Volume;
            _audioSource.pitch = data.Pitch;
            _audioSource.priority = data.Priority;
            _audioSource.panStereo = data.PanStereo;
            _audioSource.timeSamples = data.StartSample;
        }
    }
}
