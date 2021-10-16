using NUnit.Framework;

namespace UnityEngine.Advertisements.Tests
{
    [TestFixture]
    class UnsupportedPlatformTests
    {
        private UnsupportedPlatform m_Platform;

        [SetUp]
        public void SetUp()
        {
            m_Platform = new UnsupportedPlatform();
        }

        [Test]
        public void InitialState()
        {
            Assert.That(m_Platform.debugMode, Is.False);
            Assert.That(m_Platform.isInitialized, Is.False);
            Assert.That(m_Platform.isSupported, Is.False);
            Assert.That(m_Platform.version, Is.Null);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void IsReadyAndGetPlacementState(string placementId)
        {
            Assert.That(m_Platform.IsReady(placementId), Is.False);
            Assert.That(m_Platform.GetPlacementState(placementId), Is.EqualTo(PlacementState.NotAvailable));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void Load(string placementId)
        {
            Assert.DoesNotThrow(() => m_Platform.Load(placementId));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void Show(string placementId)
        {
            Assert.DoesNotThrow(() => m_Platform.Show(placementId));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void ShowWithEvent(string placementId)
        {
            int count = 0;
            object sender = null;
            FinishEventArgs eventArgs = null;
            m_Platform.OnFinish += (s, a) =>
            {
                count++;
                sender = s;
                eventArgs = a;
            };

            Assert.DoesNotThrow(() => m_Platform.Show(placementId));

            Assert.That(count, Is.EqualTo(1));
            Assert.That(sender, Is.SameAs(m_Platform));
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.placementId, Is.EqualTo(placementId));
            Assert.That(eventArgs.showResult, Is.EqualTo(ShowResult.Failed));
        }

        [TestCase(null, false, false)]
        [TestCase("", false, false)]
        [TestCase("test", false, false)]
        [TestCase("123435", false, false)]
        [TestCase(null, true, false)]
        [TestCase("", true, false)]
        [TestCase("test", true, false)]
        [TestCase("123435", true, false)]
        [TestCase(null, false, true)]
        [TestCase("", false, true)]
        [TestCase("test", false, true)]
        [TestCase("123435", false, true)]
        [TestCase(null, true, true)]
        [TestCase("", true, true)]
        [TestCase("test", true, true)]
        [TestCase("123435", true, true)]
        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
        {
            Assert.DoesNotThrow(() => m_Platform.Initialize(gameId, testMode, enablePerPlacementLoad));
        }

        [Test]
        public void SetMetaData()
        {
            Assert.DoesNotThrow(() => m_Platform.SetMetaData(new MetaData("test")));
        }

        [Test]
        public void Events()
        {
            Assert.DoesNotThrow(() => m_Platform.OnReady += (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnStart += (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnError += (X, y) => {});

            Assert.DoesNotThrow(() => m_Platform.OnReady -= (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnStart -= (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnError -= (X, y) => {});
        }
    }
}
