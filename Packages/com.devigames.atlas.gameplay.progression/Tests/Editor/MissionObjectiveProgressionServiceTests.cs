using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Objectives.Events;

using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Tests
{
    public sealed class MissionObjectiveProgressionServiceTests
    {
        private MissionCollection _missionCollection;
        private FakeMissionSessionService _sessionService;
        private MissionObjectiveProgressionService _service;

        private MissionRuntime _missionA;
        private MissionRuntime _missionB;

        private MissionCompletedEvent _lastMissionCompletedEvent;
        private MissionObjectiveCompletedEvent _lastMissionObjectiveCompletedEvent;

        private int _missionCompletedEventCount;
        private int _missionObjectiveCompletedEventCount;

        [SetUp]
        public void SetUp()
        {
            _missionCompletedEventCount = 0;
            _missionObjectiveCompletedEventCount = 0;

            _lastMissionCompletedEvent = default;
            _lastMissionObjectiveCompletedEvent = default;

            _missionCollection =
                new MissionCollection();

            _sessionService =
                new FakeMissionSessionService();

            _missionA =
                CreateMission(
                    "mission.a",
                    "Mission A",
                    "objective.shared");

            _missionB =
                CreateMission(
                    "mission.b",
                    "Mission B",
                    "objective.shared");

            _missionCollection.Add(
                _missionA);

            _missionCollection.Add(
                _missionB);

            _service =
                new MissionObjectiveProgressionService(
                    _sessionService,
                    _missionCollection);

            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);

            EventBus.Subscribe<MissionObjectiveCompletedEvent>(
                OnMissionObjectiveCompleted);

            _service.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Shutdown();

            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);

            EventBus.Unsubscribe<MissionObjectiveCompletedEvent>(
                OnMissionObjectiveCompleted);
        }

        [Test]
        public void ObjectiveCompleted_WhenNoActiveSession_DoesNotProgressAnyMission()
        {
            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionB.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void ObjectiveCompleted_WhenMissionAIsActive_ProgressesMissionA()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _missionA.IsObjectiveCompleted(
                    "objective.shared"),
                Is.True);
        }

        [Test]
        public void ObjectiveCompleted_WhenMissionAIsActive_DoesNotProgressMissionB()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _missionB.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionB.IsObjectiveCompleted(
                    "objective.shared"),
                Is.False);
        }

        [Test]
        public void SharedObjective_WhenMissionAIsActive_CompletesOnlyMissionA()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.IsCompleted,
                Is.True);

            Assert.That(
                _missionB.IsCompleted,
                Is.False);
        }

        [Test]
        public void SharedObjective_WhenMissionAIsActive_PublishesMissionCompletedForMissionAOnly()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _lastMissionCompletedEvent.MissionId,
                Is.EqualTo(
                    "mission.a"));
        }

        [Test]
        public void CompletedObjective_PublishesMissionObjectiveCompletedEventForActiveMission()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _lastMissionObjectiveCompletedEvent.MissionId,
                Is.EqualTo(
                    "mission.a"));

            Assert.That(
                _lastMissionObjectiveCompletedEvent.ObjectiveId,
                Is.EqualTo(
                    "objective.shared"));
        }

        [Test]
        public void ObjectiveBelongingOnlyToInactiveMission_DoesNotProgressInactiveMission()
        {
            MissionRuntime missionC =
                CreateMission(
                    "mission.c",
                    "Mission C",
                    "objective.c");

            _missionCollection.Add(
                missionC);

            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.c",1,1));

            Assert.That(
                missionC.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                missionC.IsCompleted,
                Is.False);

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void ObjectiveNotBelongingToActiveMission_DoesNotProgressActiveMission()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.other",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionA.IsCompleted,
                Is.False);

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void ObjectiveCompleted_WhenActiveMissionCannotBeFound_DoesNothing()
        {
            _sessionService.SetActiveMission(
                "mission.missing");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionB.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void MultiObjectiveMission_FirstObjective_ProgressesWithoutCompletingMission()
        {
            MissionRuntime mission =
                CreateMission(
                    "mission.multi",
                    "Multi Mission",
                    "objective.one",
                    "objective.two");

            _missionCollection.Add(
                mission);

            _sessionService.SetActiveMission(
                "mission.multi");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.one",1,1));

            Assert.That(
                mission.CompletedObjectiveCount,
                Is.EqualTo(
                    1));

            Assert.That(
                mission.IsCompleted,
                Is.False);

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void MultiObjectiveMission_FinalObjective_CompletesMission()
        {
            MissionRuntime mission =
                CreateMission(
                    "mission.multi",
                    "Multi Mission",
                    "objective.one",
                    "objective.two");

            _missionCollection.Add(
                mission);

            _sessionService.SetActiveMission(
                "mission.multi");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.one",1,1));

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.two",1,1));

            Assert.That(
                mission.CompletedObjectiveCount,
                Is.EqualTo(
                    2));

            Assert.That(
                mission.IsCompleted,
                Is.True);

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    2));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _lastMissionCompletedEvent.MissionId,
                Is.EqualTo(
                    "mission.multi"));
        }

        [Test]
        public void DuplicateObjectiveCompletion_DoesNotPublishDuplicateMissionEvents()
        {
            MissionRuntime mission =
                CreateMission(
                    "mission.multi",
                    "Multi Mission",
                    "objective.one",
                    "objective.two");

            _missionCollection.Add(
                mission);

            _sessionService.SetActiveMission(
                "mission.multi");

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.one",1,1));

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.one",1,1));

            Assert.That(
                mission.CompletedObjectiveCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _missionObjectiveCompletedEventCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void Shutdown_UnsubscribesFromObjectiveCompletedEvent()
        {
            _sessionService.SetActiveMission(
                "mission.a");

            _service.Shutdown();

            EventBus.Publish(
                new ObjectiveCompletedEvent(
                    "objective.shared",1,1));

            Assert.That(
                _missionA.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionB.CompletedObjectiveCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _missionCompletedEventCount,
                Is.EqualTo(
                    0));

            _service.Initialize();
        }

        private MissionRuntime CreateMission(
            string missionId,
            string displayName,
            params string[] objectiveIds)
        {
            var definition =
                new MissionDefinition(
                    missionId,
                    displayName,
                    string.Empty,
                    objectiveIds);

            return new MissionRuntime(
                definition);
        }

        private void OnMissionCompleted(
            MissionCompletedEvent eventData)
        {
            _missionCompletedEventCount++;

            _lastMissionCompletedEvent =
                eventData;
        }

        private void OnMissionObjectiveCompleted(
            MissionObjectiveCompletedEvent eventData)
        {
            _missionObjectiveCompletedEventCount++;

            _lastMissionObjectiveCompletedEvent =
                eventData;
        }

        private sealed class FakeMissionSessionService :
            IMissionSessionService
        {
            public string ActiveMissionId { get; private set; } =
                string.Empty;

            public bool HasActiveSession =>
                !string.IsNullOrWhiteSpace(
                    ActiveMissionId);

            public bool Start(
                string missionId)
            {
                if (HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    missionId;

                return true;
            }

            public bool Restart()
            {
                return HasActiveSession;
            }

            public bool Exit()
            {
                if (!HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    string.Empty;

                return true;
            }

            public void SetActiveMission(
                string missionId)
            {
                ActiveMissionId =
                    missionId;
            }
        }
    }
}