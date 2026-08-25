using UnityEngine;
using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Objectives.Collections;
using DeviGames.Atlas.Core.Objectives.Runtime;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Services;

using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Tests
{
    public class MissionSessionServiceTests 
    {
        private MissionCollection _missionCollection;
        private ObjectiveCollection _objectiveCollection;
        private IUnlockService _unlockService;
        private IMissionAvailabilityService _availabilityService;
        private MissionSessionService _sessionService;

        [SetUp]
        public void SetUp()
        {
            _missionCollection = new MissionCollection();
            _objectiveCollection = new ObjectiveCollection();
            _unlockService = new UnlockService();

            RegisterMission(
                "mission.chapter-01",
                "objective.chapter-01");

            RegisterMission(
                "mission.chapter-02",
                "objective.chapter-02");

            _unlockService.Unlock("mission.chapter-01");

            _availabilityService =
                new MissionAvailabilityService(
                    _missionCollection,
                    _unlockService);

            _sessionService =
                new MissionSessionService(
                    _missionCollection,
                    _objectiveCollection,
                    _availabilityService);
        }

        [Test]
        public void Start_UnlockedMission_StartsSession()
        {
            bool result =
                _sessionService.Start(
                    "mission.chapter-01");

            Assert.That(result, Is.True);
            Assert.That(_sessionService.HasActiveSession, Is.True);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo("mission.chapter-01"));
        }

        [Test]
        public void Start_LockedMission_ReturnsFalse()
        {
            bool result =
                _sessionService.Start(
                    "mission.chapter-02");

            Assert.That(result, Is.False);
            Assert.That(_sessionService.HasActiveSession, Is.False);
        }

        [Test]
        public void Start_UnknownMission_ReturnsFalse()
        {
            bool result =
                _sessionService.Start(
                    "mission.unknown");

            Assert.That(result, Is.False);
            Assert.That(_sessionService.HasActiveSession, Is.False);
        }

        [Test]
        public void Start_WhenSessionAlreadyActive_ReturnsFalse()
        {
            _sessionService.Start(
                "mission.chapter-01");

            _unlockService.Unlock(
                "mission.chapter-02");

            bool result =
                _sessionService.Start(
                    "mission.chapter-02");

            Assert.That(result, Is.False);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo("mission.chapter-01"));
        }

        [Test]
        public void Restart_ResetsMissionAndObjectives()
        {
            _sessionService.Start(
                "mission.chapter-01");

            _objectiveCollection.TryGet(
                "objective.chapter-01",
                out ObjectiveRuntime objective);

            _missionCollection.TryGet(
                "mission.chapter-01",
                out MissionRuntime mission);

            objective.AddProgress(3);

            mission.NotifyObjectiveCompleted(
                "objective.chapter-01");

            Assert.That(objective.IsCompleted, Is.True);
            Assert.That(mission.IsCompleted, Is.True);

            bool result =
                _sessionService.Restart();

            Assert.That(result, Is.True);

            Assert.That(objective.CurrentValue, Is.Zero);
            Assert.That(objective.IsCompleted, Is.False);

            Assert.That(mission.CompletedObjectiveCount, Is.Zero);
            Assert.That(mission.IsCompleted, Is.False);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo("mission.chapter-01"));
        }

        [Test]
        public void Exit_ResetsRuntimeAndEndsSession()
        {
            _sessionService.Start(
                "mission.chapter-01");

            _objectiveCollection.TryGet(
                "objective.chapter-01",
                out ObjectiveRuntime objective);

            objective.AddProgress(2);

            bool result =
                _sessionService.Exit();

            Assert.That(result, Is.True);

            Assert.That(objective.CurrentValue, Is.Zero);
            Assert.That(_sessionService.HasActiveSession, Is.False);
            Assert.That(_sessionService.ActiveMissionId, Is.Empty);
        }
        [Test]
        public void Exit_WithoutActiveSession_ReturnsFalse()
        {
            bool result =
                _sessionService.Exit();

            Assert.That(result, Is.False);
        }

        [Test]
        public void MissionCompleted_ActiveMission_EndsSession()
        {
            _sessionService.Initialize();

            try
            {
                _sessionService.Start("mission.chapter-01");

                EventBus.Publish(
                    new MissionCompletedEvent("mission.chapter-01"));

                Assert.That(_sessionService.HasActiveSession, Is.False);
                Assert.That(_sessionService.ActiveMissionId, Is.Empty);
            }
            finally
            {
                _sessionService.Shutdown();
            }
        }

        [Test]
        public void MissionCompleted_DifferentMission_DoesNotEndSession()
        {
            _sessionService.Initialize();

            try
            {
                _sessionService.Start("mission.chapter-01");

                EventBus.Publish(
                    new MissionCompletedEvent("mission.chapter-02"));

                Assert.That(_sessionService.HasActiveSession, Is.True);
                Assert.That(
                    _sessionService.ActiveMissionId,
                    Is.EqualTo("mission.chapter-01"));
            }
            finally
            {
                _sessionService.Shutdown();
            }
        }
        private void RegisterMission(
            string missionId,
            string objectiveId)
        {
            var objectiveDefinition =
                new ObjectiveDefinition(
                    id: objectiveId,
                    displayName: objectiveId,
                    description: "",
                    targetValue: 3);

            var objectiveRuntime =
                new ObjectiveRuntime(
                    objectiveDefinition);

            _objectiveCollection.Add(
                objectiveRuntime);

            var missionDefinition =
                new MissionDefinition(
                    id: missionId,
                    displayName: missionId,
                    description: "",
                    objectiveIds: new[]
                    {
                        objectiveId
                    });

            var missionRuntime =
                new MissionRuntime(
                    missionDefinition);

            _missionCollection.Add(
                missionRuntime);
        }
    }
}