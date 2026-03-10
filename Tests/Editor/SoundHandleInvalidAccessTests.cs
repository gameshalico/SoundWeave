#nullable enable

using System;
using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundHandleInvalidAccessTests
    {
        private MockSoundControl _control = null!;
        private SoundHandle _invalidHandle;

        [SetUp]
        public void SetUp()
        {
            _control = new MockSoundControl();
            _invalidHandle = new SoundHandle(_control);
            _control.Invalidate();
        }

        [Test]
        public void Stop_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Stop());
        }

        [Test]
        public void Pause_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Pause());
        }

        [Test]
        public void UnPause_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.UnPause());
        }

        [Test]
        public void SetScheduledStartTime_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.SetScheduledStartTime(1.0));
        }

        [Test]
        public void SetScheduledEndTime_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.SetScheduledEndTime(1.0));
        }

        [Test]
        public void Position_Get_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.Position; });
        }

        [Test]
        public void Position_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Position = Vector3.zero);
        }

        [Test]
        public void Clip_Get_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.Clip; });
        }

        [Test]
        public void Clip_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Clip = null!);
        }

        [Test]
        public void OutputAudioMixerGroup_Get_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.OutputAudioMixerGroup; });
        }

        [Test]
        public void OutputAudioMixerGroup_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.OutputAudioMixerGroup = null);
        }

        [Test]
        public void IsPlaying_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.IsPlaying; });
        }

        [Test]
        public void Time_Get_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.Time; });
        }

        [Test]
        public void Time_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Time = 0f);
        }

        [Test]
        public void Mute_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Mute = true);
        }

        [Test]
        public void Volume_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Volume = 0.5f);
        }

        [Test]
        public void Pitch_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Pitch = 1f);
        }

        [Test]
        public void Priority_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Priority = 128);
        }

        [Test]
        public void PanStereo_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.PanStereo = 0f);
        }

        [Test]
        public void TimeSamples_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.TimeSamples = 0);
        }

        [Test]
        public void Loop_Set_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _invalidHandle.Loop = true);
        }

        [Test]
        public void PlayDspTime_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.PlayDspTime; });
        }

        [Test]
        public void State_WhenInvalid_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => { _ = _invalidHandle.State; });
        }
    }
}
