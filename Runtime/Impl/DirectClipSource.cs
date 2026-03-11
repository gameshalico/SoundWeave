#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoundWeave.Impl
{
    [Serializable]
    public sealed class DirectClipSource : IClipSource
    {
        [SerializeField] private AudioClip? _clip;

        public AudioClip? Clip => _clip;
        public bool IsReady => _clip != null;

        public UniTask LoadAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public void Release()
        {
        }
    }
}
