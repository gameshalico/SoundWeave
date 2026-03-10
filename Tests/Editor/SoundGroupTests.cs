#nullable enable

using System;
using NUnit.Framework;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundGroupTests
    {
        private MockSoundControl _control = null!;
        private SoundGroup _group = null!;

        [SetUp]
        public void SetUp()
        {
            _control = new MockSoundControl();
            _group = new SoundGroup();
        }

        [Test]
        public void Count_EmptyGroup_ReturnsZero()
        {
            Assert.That(_group.Count, Is.EqualTo(0));
        }

        [Test]
        public void Add_ActiveHandle_IncreasesCount()
        {
            var handle = new SoundHandle(_control);
            _group.Add(handle);
            Assert.That(_group.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_InactiveHandle_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(
                () => _group.Add(SoundHandle.Invalid));
        }

        [Test]
        public void Remove_ExistingHandle_ReturnsTrue()
        {
            var handle = new SoundHandle(_control);
            _group.Add(handle);
            Assert.That(_group.Remove(handle), Is.True);
            Assert.That(_group.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_NonExistingHandle_ReturnsFalse()
        {
            Assert.That(_group.Remove(SoundHandle.Invalid), Is.False);
        }

        [Test]
        public void Clear_RemovesAllHandles()
        {
            _group.Add(new SoundHandle(_control));
            _group.Clear();
            Assert.That(_group.Count, Is.EqualTo(0));
        }

        [Test]
        public void StopAll_StopsActiveHandlesAndClears()
        {
            var handle = new SoundHandle(_control);
            _group.Add(handle);
            _group.StopAll();

            Assert.That(_control.StopCalled, Is.True);
            Assert.That(_group.Count, Is.EqualTo(0));
        }

        [Test]
        public void StopAll_SkipsInactiveHandles()
        {
            var handle = new SoundHandle(_control);
            _group.Add(handle);
            _control.Invalidate();

            Assert.DoesNotThrow(() => _group.StopAll());
            Assert.That(_control.StopCalled, Is.False);
        }

        [Test]
        public void Count_PrunesInactiveHandles()
        {
            _group.Add(new SoundHandle(_control));
            _control.Invalidate();
            Assert.That(_group.Count, Is.EqualTo(0));
        }

        [Test]
        public void Enumerate_ReturnsActiveHandles()
        {
            var c1 = new MockSoundControl();
            var c2 = new MockSoundControl();
            var h1 = new SoundHandle(c1);
            var h2 = new SoundHandle(c2);

            _group.Add(h1);
            _group.Add(h2);

            var count = 0;
            foreach (var handle in _group)
            {
                Assert.That(handle.IsActive(), Is.True);
                count++;
            }
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void Enumerate_PrunesInactiveHandlesBeforeIteration()
        {
            var c1 = new MockSoundControl();
            var c2 = new MockSoundControl();
            _group.Add(new SoundHandle(c1));
            _group.Add(new SoundHandle(c2));

            c1.Invalidate();

            var count = 0;
            foreach (var _ in _group)
                count++;

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void Enumerate_ModificationDuringIteration_ThrowsInvalidOperationException()
        {
            var c1 = new MockSoundControl();
            _group.Add(new SoundHandle(c1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var _ in _group)
                {
                    var c2 = new MockSoundControl();
                    _group.Add(new SoundHandle(c2));
                }
            });
        }

        [Test]
        public void Constructor_WithCapacity_DoesNotThrow()
        {
            var group = new SoundGroup(16);
            Assert.That(group.Count, Is.EqualTo(0));
        }

        [Test]
        public void Add_MultipleHandles_CountIsCorrect()
        {
            var c1 = new MockSoundControl();
            var c2 = new MockSoundControl();
            var c3 = new MockSoundControl();

            _group.Add(new SoundHandle(c1));
            _group.Add(new SoundHandle(c2));
            _group.Add(new SoundHandle(c3));

            Assert.That(_group.Count, Is.EqualTo(3));
        }
    }
}
