#nullable enable

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class SoundProfile : ISoundProfileFactory
    {
        [SerializeField] private AudioClip? _clip;
        [SerializeField] private AudioMixerGroup? _outputAudioMixerGroup;
        [SerializeField] private bool _mute;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private float _pitch = 1f;
        [SerializeField] private int _priority = 128;
        [SerializeField] private float _panStereo;
        [SerializeField] private int _startSample;
        [SerializeField] private bool _loop;
        [SerializeField, Min(0)] private double _delay;

        public SoundBuilder CreateBuilder()
        {
            return SoundBuilder.Create().WithAllParams(
                Vector3.zero, _clip, _outputAudioMixerGroup, _mute, _volume, _pitch,
                _priority, _panStereo, _startSample, _loop,
                _delay <= 0 ? TimingMode.Immediate : TimingMode.Delay, _delay);
        }

        public bool IsValid()
        {
            return _clip != null;
        }
    }
}
