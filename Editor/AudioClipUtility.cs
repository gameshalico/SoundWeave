#nullable enable

using UnityEngine;

namespace SoundWeave.Editor
{
    public static class AudioClipUtility
    {
        public static (int startSample, int endSample) DetectSilenceSamples(
            AudioClip clip, float threshold = 0.01f)
        {
            var samples = new float[clip.samples * clip.channels];
            if (!clip.GetData(samples, 0))
            {
                Debug.LogError("Failed to get data from audio clip.");
                return (0, 0);
            }

            var startSample = 0;
            var endSample = samples.Length - 1;

            for (var i = 0; i < samples.Length; i++)
            {
                if (Mathf.Abs(samples[i]) > threshold)
                {
                    startSample = i / clip.channels;
                    break;
                }
            }

            for (var i = samples.Length - 1; i >= 0; i--)
            {
                if (Mathf.Abs(samples[i]) > threshold)
                {
                    endSample = i / clip.channels;
                    break;
                }
            }

            return (startSample, endSample);
        }
    }
}
