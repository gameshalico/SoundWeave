#nullable enable

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave
{
    [Serializable]
    public readonly struct SoundData
    {
        [SerializeField] private readonly Vector3 _position;
        [SerializeField] private readonly IAudioGenerator.Serializable _audioGenerator;
        [SerializeField] private readonly AudioMixerGroup? _outputAudioMixerGroup;
        [SerializeField] private readonly bool _mute;
        [SerializeField] private readonly float _volume;
        [SerializeField] private readonly float _pitch;
        [SerializeField] private readonly int _priority;
        [SerializeField] private readonly float _panStereo;
        [SerializeField] private readonly int _startSample;
        [SerializeField] private readonly bool _loop;
        [SerializeField] private readonly TimingMode _timingMode;
        [SerializeField] private readonly double _timingValue;
        [SerializeField] private readonly double? _scheduledEndTime;

        public Vector3 Position => _position;
        public IAudioGenerator.Serializable AudioGenerator => _audioGenerator;
        public AudioMixerGroup? OutputAudioMixerGroup => _outputAudioMixerGroup;
        public bool Mute => _mute;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public int Priority => _priority;
        public float PanStereo => _panStereo;
        public int StartSample => _startSample;
        public bool Loop => _loop;
        public TimingMode TimingMode => _timingMode;
        public double TimingValue => _timingValue;
        public double? ScheduledEndTime => _scheduledEndTime;

        public SoundData(
            IAudioGenerator.Serializable audioGenerator,
            Vector3 position = default,
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
            _audioGenerator = audioGenerator;
            _position = position;
            _outputAudioMixerGroup = outputAudioMixerGroup;
            _mute = mute;
            _volume = volume;
            _pitch = pitch;
            _priority = priority;
            _panStereo = panStereo;
            _startSample = startSample;
            _loop = loop;
            _timingMode = timingMode;
            _timingValue = timingValue;
            _scheduledEndTime = scheduledEndTime;
        }
    }
}
