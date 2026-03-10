#nullable enable

using NUnit.Framework;

namespace SoundWeave.Tests
{
    [TestFixture]
    public sealed class SoundGroupExtensionsTests
    {
        [Test]
        public void AddToGroup_AddsHandleAndReturnsIt()
        {
            var control = new MockSoundControl();
            var handle = new SoundHandle(control);
            var group = new SoundGroup();

            var returned = handle.AddToGroup(group);

            Assert.That(returned, Is.EqualTo(handle));
            Assert.That(group.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddToGroup_WithMap_AddsHandleToCorrectGroup()
        {
            var control = new MockSoundControl();
            var handle = new SoundHandle(control);
            var map = new SoundGroupMap<string>();

            var returned = handle.AddToGroup(map, "sfx");

            Assert.That(returned, Is.EqualTo(handle));
            Assert.That(map["sfx"].Count, Is.EqualTo(1));
        }
    }
}
