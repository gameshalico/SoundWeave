#nullable enable

using System;
using NUnit.Framework;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundBuilderDisposedAccessTests
    {
        private SoundBuilder _disposed;

        [SetUp]
        public void SetUp()
        {
            _disposed = SoundBuilder.Create();
            _disposed.Dispose();
        }

        [Test]
        public void Clip_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.Clip; });

        [Test]
        public void OutputAudioMixerGroup_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.OutputAudioMixerGroup; });

        [Test]
        public void Mute_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.Mute; });

        [Test]
        public void Pitch_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.Pitch; });

        [Test]
        public void Priority_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.Priority; });

        [Test]
        public void PanStereo_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.PanStereo; });

        [Test]
        public void StartSample_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.StartSample; });

        [Test]
        public void Loop_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.Loop; });

        [Test]
        public void TimingMode_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.TimingMode; });

        [Test]
        public void TimingValue_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.TimingValue; });

        [Test]
        public void ScheduledEndTime_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.ScheduledEndTime; });

        [Test]
        public void PlayDspTime_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => { _ = _disposed.PlayDspTime; });

        [Test]
        public void WithVolume_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithVolume(1f));

        [Test]
        public void WithPitch_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithPitch(1f));

        [Test]
        public void WithPriority_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithPriority(128));

        [Test]
        public void WithPanStereo_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithPanStereo(0f));

        [Test]
        public void WithMute_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithMute(false));

        [Test]
        public void WithLoop_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithLoop(false));

        [Test]
        public void WithStartSample_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithStartSample(0));

        [Test]
        public void WithImmediate_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithImmediate());

        [Test]
        public void WithDelay_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithDelay(0));

        [Test]
        public void WithSchedule_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithSchedule(0));

        [Test]
        public void WithScheduledEndTime_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.WithScheduledEndTime(0));

        [Test]
        public void Play_Throws() =>
            Assert.Throws<ObjectDisposedException>(() => _disposed.Play(new MockSoundPlayer()));
    }
}
