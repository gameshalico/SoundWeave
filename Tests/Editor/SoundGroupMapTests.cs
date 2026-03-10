#nullable enable

using NUnit.Framework;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundGroupMapTests
    {
        private SoundGroupMap<string> _map = null!;

        [SetUp]
        public void SetUp()
        {
            _map = new SoundGroupMap<string>();
        }

        [Test]
        public void GetOrAdd_NewKey_CreatesGroup()
        {
            var group = _map.GetOrAdd("sfx");
            Assert.That(group, Is.Not.Null);
            Assert.That(group.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOrAdd_SameKey_ReturnsSameGroup()
        {
            var g1 = _map.GetOrAdd("sfx");
            var g2 = _map.GetOrAdd("sfx");
            Assert.That(g1, Is.SameAs(g2));
        }

        [Test]
        public void Indexer_ReturnsSameAsGetOrAdd()
        {
            var g1 = _map["bgm"];
            var g2 = _map.GetOrAdd("bgm");
            Assert.That(g1, Is.SameAs(g2));
        }

        [Test]
        public void TryGet_ExistingKey_ReturnsTrue()
        {
            _map.GetOrAdd("sfx");
            Assert.That(_map.TryGet("sfx", out var group), Is.True);
            Assert.That(group, Is.Not.Null);
        }

        [Test]
        public void TryGet_NonExistingKey_ReturnsFalse()
        {
            Assert.That(_map.TryGet("none", out _), Is.False);
        }

        [Test]
        public void Remove_ExistingKey_StopsAllAndRemovesGroup()
        {
            var control = new MockSoundControl();
            var handle = new SoundHandle(control);
            _map["sfx"].Add(handle);

            _map.Remove("sfx");

            Assert.That(control.StopCalled, Is.True);
            Assert.That(_map.TryGet("sfx", out _), Is.False);
        }

        [Test]
        public void Remove_NonExistingKey_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _map.Remove("none"));
        }

        [Test]
        public void Clear_StopsAllGroupsAndRemovesAll()
        {
            var c1 = new MockSoundControl();
            var c2 = new MockSoundControl();
            _map["sfx"].Add(new SoundHandle(c1));
            _map["bgm"].Add(new SoundHandle(c2));

            _map.Clear();

            Assert.That(c1.StopCalled, Is.True);
            Assert.That(c2.StopCalled, Is.True);
            Assert.That(_map.TryGet("sfx", out _), Is.False);
            Assert.That(_map.TryGet("bgm", out _), Is.False);
        }

        [Test]
        public void Constructor_WithCapacity_DoesNotThrow()
        {
            var map = new SoundGroupMap<int>(8);
            Assert.That(map.TryGet(0, out _), Is.False);
        }
    }
}
