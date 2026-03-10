#nullable enable
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class AddressableSoundProfile : ISoundProfileFactory
    {
        [SerializeField] private AssetReferenceT<AudioClip>? _clipReference;
        [SerializeField] private AudioMixerGroup? _outputAudioMixerGroup;
        [SerializeField] private bool _mute;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private float _pitch = 1f;
        [SerializeField] private int _priority = 128;
        [SerializeField] private float _panStereo;
        [SerializeField] private int _startSample;
        [SerializeField] private bool _loop;
        [SerializeField, Min(0)] private double _delay;

        private AudioClip? _cachedClip;
        private AsyncOperationHandle<AudioClip> _handle;
        private bool _loaded;

        public async UniTask LoadAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
                return;

            if (_clipReference == null || !_clipReference.RuntimeKeyIsValid())
                throw new InvalidOperationException("AudioClip reference is not set or invalid.");

            AsyncOperationHandle<AudioClip> handle = _clipReference.LoadAssetAsync<AudioClip>();
            try
            {
                await handle.ToUniTask(cancellationToken: cancellationToken);
                _cachedClip = handle.Result;
                _handle = handle;
                _loaded = true;
            }
            catch
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
                throw;
            }
        }

        public void Release()
        {
            if (!_loaded)
                return;

            if (_handle.IsValid())
                Addressables.Release(_handle);

            _handle = default;
            _cachedClip = null;
            _loaded = false;
        }

        public SoundBuilder CreateBuilder()
        {
            if (!_loaded || _cachedClip == null)
                throw new InvalidOperationException(
                    "AudioClip has not been loaded. Call LoadAsync() first.");

            return SoundBuilder.Create().WithAllParams(
                Vector3.zero, _cachedClip, _outputAudioMixerGroup, _mute, _volume, _pitch,
                _priority, _panStereo, _startSample, _loop,
                _delay <= 0 ? TimingMode.Immediate : TimingMode.Delay, _delay);
        }

        public bool IsValid()
        {
            return _clipReference != null && _clipReference.RuntimeKeyIsValid();
        }
    }
}
#endif
