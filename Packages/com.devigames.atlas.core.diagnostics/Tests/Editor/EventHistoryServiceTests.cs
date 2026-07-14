using DeviGames.Atlas.Core.Diagnostics.Services;
using DeviGames.Atlas.Core.Events;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Diagnostics.Tests
{
    public sealed class EventHistoryServiceTests
    {
        private EventHistoryService _history;

        [SetUp]
        public void Setup()
        {
            EventBusTestUtility.Reset();

            _history = new EventHistoryService(3);
            _history.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _history.Shutdown();
            EventBusTestUtility.Reset();
        }

        [Test]
        public void Published_Event_Should_Be_Recorded()
        {
            EventBus.Publish(new TestEvent("first"));

            Assert.AreEqual(1, _history.Count);
            Assert.AreEqual(
                nameof(TestEvent),
                _history.Records[0].EventName);
        }

        [Test]
        public void History_Should_Remove_Oldest_When_Capacity_Reached()
        {
            EventBus.Publish(new TestEvent("one"));
            EventBus.Publish(new TestEvent("two"));
            EventBus.Publish(new TestEvent("three"));
            EventBus.Publish(new TestEvent("four"));

            Assert.AreEqual(3, _history.Count);

            var firstRemaining =
                (TestEvent)_history.Records[0].EventData;

            Assert.AreEqual("two", firstRemaining.Value);
        }

        [Test]
        public void Shutdown_Should_Stop_Recording()
        {
            _history.Shutdown();

            EventBus.Publish(new TestEvent("ignored"));

            Assert.AreEqual(0, _history.Count);
        }

        [Test]
        public void Clear_Should_Remove_All_Records()
        {
            EventBus.Publish(new TestEvent("first"));

            _history.Clear();

            Assert.AreEqual(0, _history.Count);
        }

        private readonly struct TestEvent
        {
            public string Value { get; }

            public TestEvent(string value)
            {
                Value = value;
            }
        }
    }
}