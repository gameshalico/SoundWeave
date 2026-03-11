#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class DirectAudioGeneratorSource : IAudioGeneratorSource
    {
        [SerializeField] private IAudioGenerator.Serializable _audioGenerator;

        public IAudioGenerator.Serializable? AudioGenerator =>
            _audioGenerator.definition != null ? _audioGenerator : null;

        public bool IsReady => _audioGenerator.definition != null;

        public UniTask<IDisposable> LoadAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.FromResult<IDisposable>(new DummyDisposable());
        }

        public void Release()
        {
        }

        private class DummyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
