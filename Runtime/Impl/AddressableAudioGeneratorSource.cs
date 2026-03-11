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
    public sealed class AddressableAudioGeneratorSource : IAudioGeneratorSource
    {
        [SerializeField] private AssetReferenceT<AudioClip>? _clipReference;

        private IAudioGenerator.Serializable? _cachedAudioGenerator;
        private AsyncOperationHandle<AudioClip> _handle;
        private bool _loaded;

        public IAudioGenerator.Serializable? AudioGenerator => _cachedAudioGenerator;
        public bool IsReady => _loaded;

        public async UniTask LoadAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
                return;

            if (_clipReference == null || !_clipReference.RuntimeKeyIsValid())
                throw new InvalidOperationException("AudioClip reference is not set or invalid.");

            var handle = _clipReference.LoadAssetAsync<AudioClip>();
            try
            {
                await handle.ToUniTask(cancellationToken: cancellationToken);
                _cachedAudioGenerator = new IAudioGenerator.Serializable(handle.Result);
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
            _cachedAudioGenerator = null;
            _loaded = false;
        }
    }
}
#endif
