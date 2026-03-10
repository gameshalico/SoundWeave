#nullable enable

using System;
using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundBuilderTests
    {
        [Test]
        public void Create_ReturnsBuilderWithDefaults()
        {
            using var builder = SoundBuilder.Create();

            Assert.That(builder.Clip, Is.Null);
            Assert.That(builder.Volume, Is.EqualTo(1f));
            Assert.That(builder.Pitch, Is.EqualTo(1f));
            Assert.That(builder.Priority, Is.EqualTo(128));
            Assert.That(builder.PanStereo, Is.EqualTo(0f));
            Assert.That(builder.StartSample, Is.EqualTo(0));
            Assert.That(builder.Loop, Is.False);
            Assert.That(builder.Mute, Is.False);
            Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Immediate));
            Assert.That(builder.TimingValue, Is.EqualTo(0d));
            Assert.That(builder.ScheduledEndTime, Is.Null);
        }

        [Test]
        public void WithVolume_SetsVolume()
        {
            using var builder = SoundBuilder.Create().WithVolume(0.5f);
            Assert.That(builder.Volume, Is.EqualTo(0.5f));
        }

        [Test]
        public void WithPitch_SetsPitch()
        {
            using var builder = SoundBuilder.Create().WithPitch(2f);
            Assert.That(builder.Pitch, Is.EqualTo(2f));
        }

        [Test]
        public void WithPriority_SetsPriority()
        {
            using var builder = SoundBuilder.Create().WithPriority(64);
            Assert.That(builder.Priority, Is.EqualTo(64));
        }

        [Test]
        public void WithPanStereo_SetsPanStereo()
        {
            using var builder = SoundBuilder.Create().WithPanStereo(-0.5f);
            Assert.That(builder.PanStereo, Is.EqualTo(-0.5f));
        }

        [Test]
        public void WithMute_SetsMute()
        {
            using var builder = SoundBuilder.Create().WithMute(true);
            Assert.That(builder.Mute, Is.True);
        }

        [Test]
        public void WithLoop_SetsLoop()
        {
            using var builder = SoundBuilder.Create().WithLoop(true);
            Assert.That(builder.Loop, Is.True);
        }

        [Test]
        public void WithStartSample_SetsStartSample()
        {
            using var builder = SoundBuilder.Create().WithStartSample(1000);
            Assert.That(builder.StartSample, Is.EqualTo(1000));
        }

        [Test]
        public void WithPosition_SetsPosition()
        {
            var pos = new Vector3(1, 2, 3);
            using var builder = SoundBuilder.Create().WithPosition(pos);
            // Position is internal via Build, but we can verify via Build
        }

        [Test]
        public void WithDelay_SetsTimingModeAndValue()
        {
            using var builder = SoundBuilder.Create().WithDelay(1.5);
            Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Delay));
            Assert.That(builder.TimingValue, Is.EqualTo(1.5));
        }

        [Test]
        public void WithSchedule_SetsTimingModeAndValue()
        {
            using var builder = SoundBuilder.Create().WithSchedule(100.0);
            Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Schedule));
            Assert.That(builder.TimingValue, Is.EqualTo(100.0));
        }

        [Test]
        public void WithImmediate_SetsTimingMode()
        {
            using var builder = SoundBuilder.Create().WithDelay(1.0).WithImmediate();
            Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Immediate));
        }

        [Test]
        public void WithScheduledEndTime_SetsEndTime()
        {
            using var builder = SoundBuilder.Create().WithScheduledEndTime(50.0);
            Assert.That(builder.ScheduledEndTime, Is.EqualTo(50.0));
        }

        [Test]
        public void FluentChaining_SetsAllProperties()
        {
            using var builder = SoundBuilder.Create()
                .WithVolume(0.7f)
                .WithPitch(1.2f)
                .WithPriority(64)
                .WithPanStereo(0.3f)
                .WithMute(true)
                .WithLoop(true)
                .WithStartSample(500)
                .WithDelay(2.0);

            Assert.That(builder.Volume, Is.EqualTo(0.7f));
            Assert.That(builder.Pitch, Is.EqualTo(1.2f));
            Assert.That(builder.Priority, Is.EqualTo(64));
            Assert.That(builder.PanStereo, Is.EqualTo(0.3f));
            Assert.That(builder.Mute, Is.True);
            Assert.That(builder.Loop, Is.True);
            Assert.That(builder.StartSample, Is.EqualTo(500));
            Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Delay));
            Assert.That(builder.TimingValue, Is.EqualTo(2.0));
        }

        [Test]
        public void Dispose_ThenAccess_ThrowsObjectDisposedException()
        {
            var builder = SoundBuilder.Create();
            builder.Dispose();

            Assert.Throws<ObjectDisposedException>(() => { _ = builder.Volume; });
        }

        [Test]
        public void Dispose_Twice_DoesNotThrow()
        {
            var builder = SoundBuilder.Create();
            builder.Dispose();
            Assert.DoesNotThrow(() => builder.Dispose());
        }

        [Test]
        public void Play_WithoutClip_ThrowsInvalidOperationException()
        {
            var player = new MockSoundPlayer();
            Assert.Throws<InvalidOperationException>(
                () => SoundBuilder.Create().Play(player));
        }

        [Test]
        public void Play_WithClip_PlaysAndDisposesBuilder()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var player = new MockSoundPlayer();
                var builder = SoundBuilder.Create()
                    .WithClip(clip)
                    .WithVolume(0.8f)
                    .WithPitch(1.5f)
                    .WithLoop(true);

                var handle = builder.Play(player);

                Assert.That(player.PlayedData.Count, Is.EqualTo(1));
                Assert.That(player.PlayedData[0].Clip, Is.EqualTo(clip));
                Assert.That(player.PlayedData[0].Volume, Is.EqualTo(0.8f));
                Assert.That(player.PlayedData[0].Pitch, Is.EqualTo(1.5f));
                Assert.That(player.PlayedData[0].Loop, Is.True);
                Assert.That(handle.IsActive(), Is.True);

                Assert.Throws<ObjectDisposedException>(() => { _ = builder.Volume; });
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(clip);
            }
        }

        [Test]
        public void Play_IncludesPositionInData()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var player = new MockSoundPlayer();
                var pos = new Vector3(1, 2, 3);
                SoundBuilder.Create()
                    .WithClip(clip)
                    .WithPosition(pos)
                    .Play(player);

                Assert.That(player.PlayedData[0].Position, Is.EqualTo(pos));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(clip);
            }
        }

        [Test]
        public void PoolReuse_BufferIsResetToDefaults()
        {
            var builder1 = SoundBuilder.Create().WithVolume(0.3f).WithPitch(2f).WithLoop(true);
            builder1.Dispose();

            using var builder2 = SoundBuilder.Create();
            Assert.That(builder2.Volume, Is.EqualTo(1f));
            Assert.That(builder2.Pitch, Is.EqualTo(1f));
            Assert.That(builder2.Loop, Is.False);
        }
    }
}
