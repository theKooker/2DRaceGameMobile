using NUnit.Framework;

namespace UnityEngine.Advertisements.Tests
{
    [TestFixture]
    class ReadyEventArgsTests
    {
        [TestCase("ads")]
        [TestCase("intro")]
        [TestCase("test")]
        [TestCase("")]
        [TestCase(null)]
        public void Constructor(string placementId)
        {
            var eventArgs = new ReadyEventArgs(placementId);

            Assert.That(eventArgs.placementId, Is.EqualTo(placementId));
        }
    }
}
