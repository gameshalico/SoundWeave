#nullable enable

using System;
using NUnit.Framework;
using UnityEngine;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundUtilityTests
    {
        [Test]
        public void EvaluateDspTime_Schedule_ReturnsTimingValue()
        {
            var result = SoundUtility.EvaluateDspTime(TimingMode.Schedule, 42.0);
            Assert.That(result, Is.EqualTo(42.0));
        }

        [Test]
        public void EvaluateDspTime_Immediate_ReturnsDspTime()
        {
            var result = SoundUtility.EvaluateDspTime(TimingMode.Immediate, 0);
            Assert.That(result, Is.EqualTo(AudioSettings.dspTime));
        }

        [Test]
        public void EvaluateDspTime_Delay_ReturnsDspTimePlusValue()
        {
            var delay = 1.5;
            var before = AudioSettings.dspTime;
            var result = SoundUtility.EvaluateDspTime(TimingMode.Delay, delay);
            var after = AudioSettings.dspTime;

            Assert.That(result, Is.GreaterThanOrEqualTo(before + delay));
            Assert.That(result, Is.LessThanOrEqualTo(after + delay));
        }

        [Test]
        public void EvaluateDspTime_InvalidMode_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => SoundUtility.EvaluateDspTime((TimingMode)999, 0));
        }
    }
}
