#nullable enable

using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundDataTests
    {
        [Test]
        public void Default_HasExpectedValues()
        {
            var data = new SoundData();

            Assert.That(data.Position, Is.EqualTo(Vector3.zero));
            Assert.That(data.Clip, Is.Null);
            Assert.That(data.OutputAudioMixerGroup, Is.Null);
            Assert.That(data.Mute, Is.False);
            Assert.That(data.Volume, Is.EqualTo(0f));
            Assert.That(data.Pitch, Is.EqualTo(0f));
            Assert.That(data.Priority, Is.EqualTo(0));
            Assert.That(data.PanStereo, Is.EqualTo(0f));
            Assert.That(data.StartSample, Is.EqualTo(0));
            Assert.That(data.Loop, Is.False);
            Assert.That(data.TimingMode, Is.EqualTo(TimingMode.Immediate));
            Assert.That(data.TimingValue, Is.EqualTo(0d));
            Assert.That(data.ScheduledEndTime, Is.Null);
        }

        [Test]
        public void Init_SetsAllProperties()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var data = new SoundData
                {
                    Clip = clip,
                    Volume = 0.8f,
                    Pitch = 1.2f,
                    Priority = 64,
                    PanStereo = -0.5f,
                    Mute = true,
                    Loop = true,
                    StartSample = 100,
                    TimingMode = TimingMode.Delay,
                    TimingValue = 2.0,
                    ScheduledEndTime = 10.0,
                    Position = new Vector3(1, 2, 3),
                };

                Assert.That(data.Clip, Is.EqualTo(clip));
                Assert.That(data.Volume, Is.EqualTo(0.8f));
                Assert.That(data.Pitch, Is.EqualTo(1.2f));
                Assert.That(data.Priority, Is.EqualTo(64));
                Assert.That(data.PanStereo, Is.EqualTo(-0.5f));
                Assert.That(data.Mute, Is.True);
                Assert.That(data.Loop, Is.True);
                Assert.That(data.StartSample, Is.EqualTo(100));
                Assert.That(data.TimingMode, Is.EqualTo(TimingMode.Delay));
                Assert.That(data.TimingValue, Is.EqualTo(2.0));
                Assert.That(data.ScheduledEndTime, Is.EqualTo(10.0));
                Assert.That(data.Position, Is.EqualTo(new Vector3(1, 2, 3)));
            }
            finally
            {
                Object.DestroyImmediate(clip);
            }
        }

        [Test]
        public void ToBuilder_CreatesBuilderWithMatchingValues()
        {
            var clip = AudioClip.Create("test", 44100, 1, 44100, false);
            try
            {
                var data = new SoundData
                {
                    Clip = clip,
                    Volume = 0.6f,
                    Pitch = 1.3f,
                    Priority = 32,
                    PanStereo = 0.7f,
                    Mute = true,
                    Loop = true,
                    StartSample = 200,
                    TimingMode = TimingMode.Schedule,
                    TimingValue = 50.0,
                    ScheduledEndTime = 60.0,
                };

                using var builder = data.ToBuilder();

                Assert.That(builder.Clip, Is.EqualTo(clip));
                Assert.That(builder.Volume, Is.EqualTo(0.6f));
                Assert.That(builder.Pitch, Is.EqualTo(1.3f));
                Assert.That(builder.Priority, Is.EqualTo(32));
                Assert.That(builder.PanStereo, Is.EqualTo(0.7f));
                Assert.That(builder.Mute, Is.True);
                Assert.That(builder.Loop, Is.True);
                Assert.That(builder.StartSample, Is.EqualTo(200));
                Assert.That(builder.TimingMode, Is.EqualTo(TimingMode.Schedule));
                Assert.That(builder.TimingValue, Is.EqualTo(50.0));
                Assert.That(builder.ScheduledEndTime, Is.EqualTo(60.0));
            }
            finally
            {
                Object.DestroyImmediate(clip);
            }
        }
    }
}
