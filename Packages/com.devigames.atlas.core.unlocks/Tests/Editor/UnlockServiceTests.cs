using System.Collections.Generic;
using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Unlocks.Events;
using DeviGames.Atlas.Core.Unlocks.Services;
using DeviGames.Atlas.Core.Unlocks.Models;

namespace DeviGames.Atlas.Core.Unlocks.Tests
{
    public sealed class UnlockServiceTests
    {
        [Test]
        public void Unlock_NewId_ReturnsTrue()
        {
            var service = new UnlockService();

            bool changed =
                service.Unlock("mission.chapter-02");

            Assert.That(changed, Is.True);
            Assert.That(
                service.IsUnlocked("mission.chapter-02"),
                Is.True);
        }

        [Test]
        public void Unlock_SameIdTwice_ReturnsFalseSecondTime()
        {
            var service = new UnlockService();

            service.Unlock("mission.chapter-02");

            bool changed =
                service.Unlock("mission.chapter-02");

            Assert.That(changed, Is.False);
        }

        [Test]
        public void IsUnlocked_UnknownId_ReturnsFalse()
        {
            var service = new UnlockService();

            Assert.That(
                service.IsUnlocked("mission.unknown"),
                Is.False);
        }

        [Test]
        public void Unlock_NewId_PublishesUnlockGrantedEvent()
        {
            var service = new UnlockService();

            UnlockGrantedEvent? received = null;

            void Handler(UnlockGrantedEvent eventData)
            {
                received = eventData;
            }

            EventBus.Subscribe<UnlockGrantedEvent>(Handler);

            try
            {
                service.Unlock("mission.chapter-02");

                Assert.That(received.HasValue, Is.True);
                Assert.That(
                    received.Value.UnlockId,
                    Is.EqualTo("mission.chapter-02"));
            }
            finally
            {
                EventBus.Unsubscribe<UnlockGrantedEvent>(Handler);
            }
        }

        [Test]
        public void Unlock_SameIdTwice_PublishesOnce()
        {
            var service = new UnlockService();

            int eventCount = 0;

            void Handler(UnlockGrantedEvent eventData)
            {
                eventCount++;
            }

            EventBus.Subscribe<UnlockGrantedEvent>(Handler);

            try
            {
                service.Unlock("mission.chapter-02");
                service.Unlock("mission.chapter-02");

                Assert.That(eventCount, Is.EqualTo(1));
            }
            finally
            {
                EventBus.Unsubscribe<UnlockGrantedEvent>(Handler);
            }
        }

        [Test]
        public void SnapshotAndLoad_RestoresUnlocks()
        {
            var source = new UnlockService();

            source.Unlock("mission.chapter-01");
            source.Unlock("mission.chapter-02");

            UnlockData data =
                source.CreateSnapshot();

            var restored = new UnlockService();

            restored.Load(data);

            Assert.That(
                restored.IsUnlocked("mission.chapter-01"),
                Is.True);

            Assert.That(
                restored.IsUnlocked("mission.chapter-02"),
                Is.True);
        }

        [Test]
        public void Load_ReplacesExistingUnlocks()
        {
            var service = new UnlockService();

            service.Unlock("mission.old");

            var data =
                new UnlockData
                {
                    UnlockedIds =
                        new List<string>
                        {
                            "mission.new"
                        }
                };

            service.Load(data);

            Assert.That(
                service.IsUnlocked("mission.old"),
                Is.False);

            Assert.That(
                service.IsUnlocked("mission.new"),
                Is.True);
        }

        [Test]
        public void Load_DoesNotPublishUnlockGrantedEvent()
        {
            var service = new UnlockService();

            int eventCount = 0;

            void Handler(
                UnlockGrantedEvent eventData)
            {
                eventCount++;
            }

            EventBus.Subscribe<UnlockGrantedEvent>(
                Handler);

            try
            {
                service.Load(
                    new UnlockData
                    {
                        UnlockedIds =
                            new List<string>
                            {
                                "mission.chapter-02"
                            }
                    });

                Assert.That(
                    eventCount,
                    Is.EqualTo(0));
            }
            finally
            {
                EventBus.Unsubscribe<UnlockGrantedEvent>(
                    Handler);
            }
        }
    }
}