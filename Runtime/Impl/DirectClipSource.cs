#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class DirectClipSource : IClipSource
    {
        [SerializeField] private IAudioGenerator.Serializable _audioGenerator;

        public IAudioGenerator.Serializable? AudioGenerator =>
            _audioGenerator.definition != null ? _audioGenerator : null;

        public bool IsReady => _audioGenerator.definition != null;

        public UniTask LoadAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public void Release()
        {
        }
    }
}
