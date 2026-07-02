using NUnit.Framework;

namespace DeviGames.Atlas.Core.Events.Tests
{
    public class EventBusTests
    {
        [SetUp]
        public void SetUp()
        {
            EventBusProvider.Reset();
        }

        // This runs automatically AFTER every single individual [Test]
        [TearDown]
        public void TearDown()
        {
            EventBusProvider.Reset();
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

        // Test 2: Nobody subscribed -> Publish event -> Nothing happens, No exception.
        [Test]
        public void Publish_WithNoSubscribers_DoesNotThrowException()
        {
            // Arrange & Act & Assert
            // We just call Publish directly. If it crashes, the test fails.
            Assert.DoesNotThrow(() => EventBus.Publish(new TestEvent(5)));
        }

        // Test 3: Listener subscribed -> Listener unsubscribed -> Publish -> Listener NOT called
        [Test]
        public void Unsubscribe_ThenPublish_ListenerNotCalled()
        {
            // Arrange
            int callCount = 0;
            void Listener(TestEvent e) => callCount++;

            EventBus.Subscribe<TestEvent>(Listener);
            EventBus.Unsubscribe<TestEvent>(Listener);

            // Act
            EventBus.Publish(new TestEvent(5));

            // Assert
            Assert.AreEqual(0, callCount);
        }

        // Test 4: Duplicate subscription -> Subscribe, Subscribe again, Publish -> One callback.
        [Test]
        public void Subscribe_TwiceWithSameListener_OnlyCalledOnce()
        {
            // Arrange
            int callCount = 0;
            void Listener(TestEvent e) => callCount++;

            EventBus.Subscribe<TestEvent>(Listener);
            EventBus.Subscribe<TestEvent>(Listener); // Duplicate

            // Act
            EventBus.Publish(new TestEvent(5));

            // Assert
            Assert.AreEqual(1, callCount);
        }

        // Test 5: Two listeners -> Listener A, Listener B, Publish -> Both called
        [Test]
        public void Publish_WithMultipleSubscribers_InvokesAllListeners()
        {
            // Arrange
            bool listenerACalled = false;
            bool listenerBCalled = false;

            void ListenerA(TestEvent e) => listenerACalled = true;
            void ListenerB(TestEvent e) => listenerBCalled = true;

            EventBus.Subscribe<TestEvent>(ListenerA);
            EventBus.Subscribe<TestEvent>(ListenerB);

            // Act
            EventBus.Publish(new TestEvent(5));

            // Assert
            Assert.IsTrue(listenerACalled, "Listener A should have been called.");
            Assert.IsTrue(listenerBCalled, "Listener B should have been called.");
        }
    }
}