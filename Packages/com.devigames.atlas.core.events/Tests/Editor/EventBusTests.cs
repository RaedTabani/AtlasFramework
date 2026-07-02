using NUnit.Framework;

namespace DeviGames.Atlas.Core.Events.Tests
{
    public class EventBusTests
    {
        [SetUp]
        public void SetUp()
        {
            EventBus.Clear();
        }

        // This runs automatically AFTER every single individual [Test]
        [TearDown]
        public void TearDown()
        {
            EventBus.Clear();
        }

        [Test]
        public void Subscribe_ThenPublish_ListenerCalledOnce()
        {
            // Arrange

            int callCount = 0;

            void Listener(TestEvent e)
            {
                callCount++;
            }

            EventBus.Subscribe<TestEvent>(Listener);

            // Act

            EventBus.Publish(new TestEvent(5));

            // Assert

            Assert.AreEqual(1, callCount);

            EventBus.Unsubscribe<TestEvent>(Listener);
        }
    }
}