#nullable enable

using UnityEngine;

namespace SoundWeave
{
    public static class SoundUtility
    {
        public static double EvaluateDspTime(TimingMode timingMode, double timingValue)
        {
            return timingMode switch
            {
                TimingMode.Immediate => AudioSettings.dspTime,
                TimingMode.Delay => AudioSettings.dspTime + timingValue,
                TimingMode.Schedule => timingValue,
                _ => throw new System.ArgumentOutOfRangeException(nameof(timingMode), timingMode, null)
            };
        }
    }
}
