#nullable enable

using System;
using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundHandleTests
    {
        private MockSoundControl _control = null!;

        [SetUp]
        public void SetUp()
        {
            _control = new MockSoundControl();
        }

        [Test]
        public void Invalid_IsNotActive()
        {
            var handle = SoundHandle.Invalid;
            Assert.That(handle.IsActive(), Is.False);
        }

        [Test]
        public void Invalid_AccessProperty_ThrowsInvalidOperationException()
        {
            var handle = SoundHandle.Invalid;
            Assert.Throws<InvalidOperationException>(() => { _ = handle.Volume; });
        }

        [Test]
        public void Constructor_CreatesActiveHandle()
        {
            var handle = new SoundHandle(_control);
            Assert.That(handle.IsActive(), Is.True);
        }

        [Test]
        public void IsActive_ReturnsFalse_AfterControlInvalidated()
        {
            var handle = new SoundHandle(_control);
            _control.Invalidate();
            Assert.That(handle.IsActive(), Is.False);
        }

        [Test]
        public void AccessProperty_AfterInvalidation_ThrowsInvalidOperationException()
        {
            var handle = new SoundHandle(_control);
            _control.Invalidate();
            Assert.Throws<InvalidOperationException>(() => { _ = handle.Volume; });
        }

        [Test]
        public void Volume_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Volume = 0.5f;
            Assert.That(handle.Volume, Is.EqualTo(0.5f));
            Assert.That(_control.Volume, Is.EqualTo(0.5f));
        }

        [Test]
        public void Pitch_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Pitch = 2f;
            Assert.That(handle.Pitch, Is.EqualTo(2f));
        }

        [Test]
        public void Priority_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Priority = 64;
            Assert.That(handle.Priority, Is.EqualTo(64));
        }

        [Test]
        public void PanStereo_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.PanStereo = -1f;
            Assert.That(handle.PanStereo, Is.EqualTo(-1f));
        }

        [Test]
        public void Mute_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Mute = true;
            Assert.That(handle.Mute, Is.True);
        }

        [Test]
        public void Loop_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Loop = true;
            Assert.That(handle.Loop, Is.True);
        }

        [Test]
        public void Position_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            var pos = new Vector3(1, 2, 3);
            handle.Position = pos;
            Assert.That(handle.Position, Is.EqualTo(pos));
        }

        [Test]
        public void TimeSamples_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.TimeSamples = 44100;
            Assert.That(handle.TimeSamples, Is.EqualTo(44100));
        }

        [Test]
        public void Time_GetSet_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Time = 1.5f;
            Assert.That(handle.Time, Is.EqualTo(1.5f));
        }

        [Test]
        public void IsPlaying_DelegatesToControl()
        {
            _control.IsPlaying = true;
            var handle = new SoundHandle(_control);
            Assert.That(handle.IsPlaying, Is.True);
        }

        [Test]
        public void State_DelegatesToControl()
        {
            _control.State = PlaybackState.Playing;
            var handle = new SoundHandle(_control);
            Assert.That(handle.State, Is.EqualTo(PlaybackState.Playing));
        }

        [Test]
        public void PlayDspTime_DelegatesToControl()
        {
            _control.PlayDspTime = 42.0;
            var handle = new SoundHandle(_control);
            Assert.That(handle.PlayDspTime, Is.EqualTo(42.0));
        }

        [Test]
        public void Stop_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Stop();
            Assert.That(_control.StopCalled, Is.True);
        }

        [Test]
        public void Pause_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.Pause();
            Assert.That(_control.PauseCalled, Is.True);
        }

        [Test]
        public void UnPause_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.UnPause();
            Assert.That(_control.UnPauseCalled, Is.True);
        }

        [Test]
        public void SetScheduledStartTime_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.SetScheduledStartTime(10.0);
            Assert.That(_control.ScheduledStartTime, Is.EqualTo(10.0));
        }

        [Test]
        public void SetScheduledEndTime_DelegatesToControl()
        {
            var handle = new SoundHandle(_control);
            handle.SetScheduledEndTime(20.0);
            Assert.That(_control.ScheduledEndTime, Is.EqualTo(20.0));
        }

        [Test]
        public void Equals_SameControlAndVersion_ReturnsTrue()
        {
            var a = new SoundHandle(_control);
            var b = new SoundHandle(_control);
            Assert.That(a.Equals(b), Is.True);
            Assert.That(a == b, Is.True);
        }

        [Test]
        public void Equals_DifferentControl_ReturnsFalse()
        {
            var other = new MockSoundControl();
            var a = new SoundHandle(_control);
            var b = new SoundHandle(other);
            Assert.That(a.Equals(b), Is.False);
            Assert.That(a != b, Is.True);
        }

        [Test]
        public void Equals_DifferentVersion_ReturnsFalse()
        {
            var a = new SoundHandle(_control);
            _control.Invalidate();
            var b = new SoundHandle(_control);
            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void Equals_InvalidHandles_AreEqual()
        {
            Assert.That(SoundHandle.Invalid.Equals(SoundHandle.Invalid), Is.True);
        }

        [Test]
        public void GetHashCode_SameHandles_AreEqual()
        {
            var a = new SoundHandle(_control);
            var b = new SoundHandle(_control);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void Equals_BoxedObject_Works()
        {
            var a = new SoundHandle(_control);
            object b = new SoundHandle(_control);
            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void Equals_NonSoundHandle_ReturnsFalse()
        {
            var a = new SoundHandle(_control);
            Assert.That(a.Equals("not a handle"), Is.False);
        }
    }
}
