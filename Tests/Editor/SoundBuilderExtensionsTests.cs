#nullable enable

using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundBuilderExtensionsTests
    {
        [Test]
        public void Play_DelegatesToPlayer()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var player = new MockSoundPlayer();
                var handle = SoundBuilder.Create()
                    .WithAudioGenerator(new IAudioGenerator.Serializable(clip))
                    .WithVolume(0.7f)
                    .Play(player);

                Assert.That(player.PlayedData.Count, Is.EqualTo(1));
                Assert.That(player.PlayedData[0].AudioGenerator, Is.EqualTo(new IAudioGenerator.Serializable(clip)));
                Assert.That(player.PlayedData[0].Volume, Is.EqualTo(0.7f));
                Assert.That(handle.IsActive(), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(clip);
            }
        }

        [Test]
        public void WithRandomVolume_SetsVolumeInRange()
        {
            using var builder = SoundBuilder.Create().WithRandomVolume(0.2f, 0.8f);
            Assert.That(builder.Volume, Is.InRange(0.2f, 0.8f));
        }

        [Test]
        public void WithRandomPitch_SetsPitchInRange()
        {
            using var builder = SoundBuilder.Create().WithRandomPitch(0.5f, 1.5f);
            Assert.That(builder.Pitch, Is.InRange(0.5f, 1.5f));
        }
    }
}
