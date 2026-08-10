using System.Collections.Generic;

using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Factories;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Events;

namespace DeviGames.Atlas.Core.Missions.Tests
{
    public sealed class MissionServiceTests
    {
        private MissionCollection _collection;
        private MissionFactory _factory;
        private MissionService _service;

        [SetUp]
        public void SetUp()
        {
            _collection =
                new MissionCollection();

            _factory =
                new MissionFactory();

            _service =
                new MissionService(
                    _factory,
                    _collection);

            _service.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Shutdown();
        }

        [Test]
        public void Register_AddsMissionToCollection()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            Assert.That(
                _collection.Count,
                Is.EqualTo(1));

            Assert.That(
                _collection.Get(
                    "mission.test"),
                Is.SameAs(runtime));
        }

        [Test]
        public void ObjectiveCompleted_UnrelatedObjective_DoesNothing()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.unrelated"));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.Zero);

            Assert.That(
                runtime.IsCompleted,
                Is.False);
        }

        [Test]
        public void ObjectiveCompleted_MissionObjective_UpdatesMission()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.a"));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(1));

            Assert.That(
                runtime.IsObjectiveCompleted(
                    "objective.a"),
                Is.True);

            Assert.That(
                runtime.IsCompleted,
                Is.False);
        }

        [Test]
        public void ObjectiveCompleted_MissionObjective_PublishesMissionObjectiveCompletedEvent()
        {
            _service.Register(
                CreateDefinition());

            MissionObjectiveCompletedEvent?
                receivedEvent = null;

            void Handler(
                MissionObjectiveCompletedEvent eventData)
            {
                receivedEvent =
                    eventData;
            }

            EventBus.Subscribe<
                MissionObjectiveCompletedEvent>(
                    Handler);

            try
            {
                EventBus.Publish(
                    CreateObjectiveCompletedEvent(
                        "objective.a"));

                Assert.That(
                    receivedEvent.HasValue,
                    Is.True);

                Assert.That(
                    receivedEvent.Value.MissionId,
                    Is.EqualTo(
                        "mission.test"));

                Assert.That(
                    receivedEvent.Value.ObjectiveId,
                    Is.EqualTo(
                        "objective.a"));

                Assert.That(
                    receivedEvent.Value
                        .CompletedObjectiveCount,
                    Is.EqualTo(1));

                Assert.That(
                    receivedEvent.Value
                        .ObjectiveCount,
                    Is.EqualTo(2));
            }
            finally
            {
                EventBus.Unsubscribe<
                    MissionObjectiveCompletedEvent>(
                        Handler);
            }
        }

        [Test]
        public void FinalObjectiveCompleted_CompletesMission()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.a"));

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.b"));

            Assert.That(
                runtime.IsCompleted,
                Is.True);

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(2));
        }

        [Test]
        public void FinalObjectiveCompleted_PublishesMissionCompletedEvent()
        {
            _service.Register(
                CreateDefinition());

            MissionCompletedEvent?
                receivedEvent = null;

            void Handler(
                MissionCompletedEvent eventData)
            {
                receivedEvent =
                    eventData;
            }

            EventBus.Subscribe<
                MissionCompletedEvent>(
                    Handler);

            try
            {
                EventBus.Publish(
                    CreateObjectiveCompletedEvent(
                        "objective.a"));

                EventBus.Publish(
                    CreateObjectiveCompletedEvent(
                        "objective.b"));

                Assert.That(
                    receivedEvent.HasValue,
                    Is.True);

                Assert.That(
                    receivedEvent.Value.MissionId,
                    Is.EqualTo(
                        "mission.test"));
            }
            finally
            {
                EventBus.Unsubscribe<
                    MissionCompletedEvent>(
                        Handler);
            }
        }

        [Test]
        public void DuplicateObjectiveCompleted_DoesNotProgressMissionTwice()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.a"));

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.a"));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(1));

            Assert.That(
                runtime.IsCompleted,
                Is.False);
        }

        [Test]
        public void FinalObjectiveCompleted_PublishesEventsInCorrectOrder()
        {
            _service.Register(
                CreateDefinition());

            var events =
                new List<string>();

            void ObjectiveHandler(
                MissionObjectiveCompletedEvent eventData)
            {
                if (eventData.ObjectiveId ==
                    "objective.b")
                {
                    events.Add(
                        "ObjectiveCompleted");
                }
            }

            void MissionHandler(
                MissionCompletedEvent eventData)
            {
                events.Add(
                    "MissionCompleted");
            }

            EventBus.Subscribe<
                MissionObjectiveCompletedEvent>(
                    ObjectiveHandler);

            EventBus.Subscribe<
                MissionCompletedEvent>(
                    MissionHandler);

            try
            {
                EventBus.Publish(
                    CreateObjectiveCompletedEvent(
                        "objective.a"));

                events.Clear();

                EventBus.Publish(
                    CreateObjectiveCompletedEvent(
                        "objective.b"));

                Assert.That(
                    events,
                    Is.EqualTo(
                        new[]
                        {
                            "ObjectiveCompleted",
                            "MissionCompleted"
                        }));
            }
            finally
            {
                EventBus.Unsubscribe<
                    MissionObjectiveCompletedEvent>(
                        ObjectiveHandler);

                EventBus.Unsubscribe<
                    MissionCompletedEvent>(
                        MissionHandler);
            }
        }
        [Test]
        public void Shutdown_StopsListeningToObjectiveEvents()
        {
            MissionRuntime runtime =
                _service.Register(
                    CreateDefinition());

            _service.Shutdown();

            EventBus.Publish(
                CreateObjectiveCompletedEvent(
                    "objective.a"));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.Zero);

            // Restore the normal test lifecycle so TearDown
            // can safely call Shutdown again.
            _service.Initialize();
        }

        private static MissionDefinition
            CreateDefinition()
        {
            return new MissionDefinition(
                id:
                    "mission.test",
                displayName:
                    "Test Mission",
                description:
                    "Test mission.",
                objectiveIds:
                    new[]
                    {
                        "objective.a",
                        "objective.b"
                    });
        }

        private static ObjectiveCompletedEvent
            CreateObjectiveCompletedEvent(
                string objectiveId)
        {
            return new ObjectiveCompletedEvent(
                objectiveId, 1,1);
        }
    }
}