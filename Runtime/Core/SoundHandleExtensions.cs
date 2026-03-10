#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoundWeave
{
    public static class SoundHandleExtensions
    {
        public static double ElapsedTime(this SoundHandle self)
        {
            return AudioSettings.dspTime - self.PlayDspTime;
        }

        public static CancellationTokenRegistration WithCancellation(
            this SoundHandle self,
            CancellationToken ct,
            SoundCancellationMode mode = SoundCancellationMode.None)
        {
            return ct.Register(() =>
            {
                if (!self.IsActive())
                    return;

                switch (mode)
                {
                    case SoundCancellationMode.Stop:
                        self.Stop();
                        break;
                    case SoundCancellationMode.Pause:
                        self.Pause();
                        break;
                }
            });
        }

        public static async UniTask ToUniTask(
            this SoundHandle self,
            CancellationToken ct = default,
            SoundCancellationMode mode = SoundCancellationMode.None)
        {
            try
            {
                await UniTask.WaitWhile(self.IsActive, cancellationToken: ct);
            }
            catch (OperationCanceledException)
            {
                if (self.IsActive())
                {
                    switch (mode)
                    {
                        case SoundCancellationMode.Stop:
                            self.Stop();
                            break;
                        case SoundCancellationMode.Pause:
                            self.Pause();
                            break;
                    }
                }

                throw;
            }
        }
    }
}
