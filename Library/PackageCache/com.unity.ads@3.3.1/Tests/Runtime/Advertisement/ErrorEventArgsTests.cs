using NUnit.Framework;

namespace UnityEngine.Advertisements.Tests
{
    [TestFixture]
    class ErrorEventArgsTests
    {
        [TestCase(0, "message")]
        [TestCase(1, "error text")]
        [TestCase(2, "")]
        [TestCase(3, null)]
        public void Constructor(long error, string message)
        {
            var eventArgs = new ErrorEventArgs(error, message);

            Assert.That(eventArgs.error, Is.EqualTo(error));
            Assert.That(eventArgs.message, Is.EqualTo(message));
        }
    }
}
