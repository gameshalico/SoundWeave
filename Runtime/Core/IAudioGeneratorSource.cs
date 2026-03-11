#nullable enable

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

namespace SoundWeave
{
    public interface IAudioGeneratorSource
    {
        IAudioGenerator.Serializable? AudioGenerator { get; }
        bool IsReady { get; }
        UniTask LoadAsync(CancellationToken cancellationToken = default);
        void Release();
    }
}
