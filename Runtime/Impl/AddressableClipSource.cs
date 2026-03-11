#nullable enable
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class AddressableClipSource : IClipSource
    {
        [SerializeField] private AssetReferenceT<AudioClip>? _clipReference;

        private AudioClip? _cachedClip;
        private AsyncOperationHandle<AudioClip> _handle;
        private bool _loaded;

        public AudioClip? Clip => _cachedClip;
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
    }
}
#endif
