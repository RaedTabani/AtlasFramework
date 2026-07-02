using NUnit.Framework;
using System;

namespace DeviGames.Atlas.Core.Events.Tests
{
    public class EventBusTests
    {
        private int _callCount;

        private void OnTestEvent(TestEvent e)
        {
            _callCount++;
        }

        [SetUp]
        public void Setup()
        {
            _callCount = 0;
            EventBusTestUtility.Reset();
        }

        [Test]
        public void Publish_Should_Invoke_Subscribed_Listener()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);

            EventBus.Publish(new TestEvent(1));

            Assert.AreEqual(1, _callCount);

            EventBus.Unsubscribe<TestEvent>(OnTestEvent);
        }

        [Test]
        public void Publish_Should_Not_Invoke_When_No_Subscribers()
        {
            EventBus.Publish(new TestEvent(1));

            Assert.AreEqual(0, _callCount);
        }

        [Test]
        public void Unsubscribe_Should_Stop_Invocation()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);

            EventBus.Publish(new TestEvent(1));

            Assert.AreEqual(0, _callCount);
        }

        [Test]
        public void Duplicate_Subscribe_Should_Not_Invoke_Twice()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);
            EventBus.Subscribe<TestEvent>(OnTestEvent);

            EventBus.Publish(new TestEvent(1));

            Assert.AreEqual(1, _callCount);
        }

        [Test]
        public void Multiple_Subscribers_Should_All_Be_Invoked()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);

            EventBus.Subscribe<TestEvent>((e) => _callCount++);
            EventBus.Subscribe<TestEvent>((e) => _callCount++);

            EventBus.Publish(new TestEvent(1));

            Assert.AreEqual(3, _callCount);
        }
    }
}