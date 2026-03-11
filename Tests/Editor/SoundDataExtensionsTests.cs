#nullable enable

using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundDataExtensionsTests
    {
        [Test]
        public void Play_DelegatesToPlayer()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var data = new SoundData(new IAudioGenerator.Serializable(clip), volume: 0.5f);
                var player = new MockSoundPlayer();

                var handle = data.Play(player);

                Assert.That(player.PlayedData.Count, Is.EqualTo(1));
                Assert.That(player.PlayedData[0].Volume, Is.EqualTo(0.5f));
                Assert.That(handle.IsActive(), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(clip);
            }
        }
    }
}
