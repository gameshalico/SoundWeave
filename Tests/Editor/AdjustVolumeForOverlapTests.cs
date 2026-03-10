#nullable enable

using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class AdjustVolumeForOverlapTests
    {
        [Test]
        public void EmptyGroup_VolumeUnchanged()
        {
            var group = new SoundGroup();
            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(1f));
        }

        [Test]
        public void IdenticalDspTime_VolumeBecomes0()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(0f));
        }

        [Test]
        public void SmallTimeDifference_Under25ms_VolumeBecomes0()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.02)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(0f));
        }

        [Test]
        public void TimeDifference_Between25msAnd50ms_VolumeReducedBy80Percent()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.03)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void TimeDifference_Between50msAnd100ms_VolumeReducedBy90Percent()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.06)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(0.9f).Within(0.001f));
        }

        [Test]
        public void TimeDifference_Over100ms_VolumeUnchanged()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.2)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(1f));
        }

        [Test]
        public void InactiveHandle_IsIgnored()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);
            control.Invalidate();

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(1f));
        }

        [Test]
        public void ZeroVolumeHandle_IsIgnored()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 0f };
            var handle = new SoundHandle(control);
            var group = new SoundGroup();
            group.Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(group);

            Assert.That(builder.Volume, Is.EqualTo(1f));
        }

        [Test]
        public void MultipleOverlaps_VolumeCompounds()
        {
            var c1 = new MockSoundControl { PlayDspTime = 10.03, Volume = 1f };
            var c2 = new MockSoundControl { PlayDspTime = 10.06, Volume = 1f };
            var group = new SoundGroup();
            group.Add(new SoundHandle(c1));
            group.Add(new SoundHandle(c2));

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(group);

            // c1: diff=0.03 → *0.8, c2: diff=0.06 → *0.9 → 1.0 * 0.8 * 0.9 = 0.72
            Assert.That(builder.Volume, Is.EqualTo(0.72f).Within(0.001f));
        }

        [Test]
        public void WithMap_DelegatesToGroup()
        {
            var control = new MockSoundControl { PlayDspTime = 10.0, Volume = 1f };
            var handle = new SoundHandle(control);
            var map = new SoundGroupMap<string>();
            map["sfx"].Add(handle);

            using var builder = SoundBuilder.Create()
                .WithVolume(1f)
                .WithSchedule(10.0)
                .AdjustVolumeForOverlap(map, "sfx");

            Assert.That(builder.Volume, Is.EqualTo(0f));
        }
    }
}
