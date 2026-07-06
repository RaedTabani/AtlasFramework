using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Definitions;
using DeviGames.Atlas.Core.Objectives.Services;
using NUnit.Framework;
using UnityEngine;

namespace DeviGames.Atlas.Core.Missions.Tests
{
    public class MissionTests
    {
        private MissionService _missionService;
        private ObjectiveService _objectiveService;

        private MissionDefinition _mission;
        private ObjectiveDefinition _objectiveA;
        private ObjectiveDefinition _objectiveB;

        private int _missionCompletedCount;
        private string _completedMissionId;

        [SetUp]
        public void Setup()
        {
            EventBusTestUtility.Reset();

            _objectiveService = new ObjectiveService();
            _missionService = new MissionService(_objectiveService);
            _missionService.Initialize();

            _objectiveA = ScriptableObject.CreateInstance<ObjectiveDefinition>();
            _objectiveB = ScriptableObject.CreateInstance<ObjectiveDefinition>();

            _objectiveA.Editor_InitializeForTests(
                "objective_a",
                "Objective A",
                "First objective",
                1);

            _objectiveB.Editor_InitializeForTests(
                "objective_b",
                "Objective B",
                "Second objective",
                1);

            _mission = ScriptableObject.CreateInstance<MissionDefinition>();
            _mission.Editor_InitializeForTests(
                "mission_001",
                "Mission 001",
                "Mission with two objectives",
                "TestScene",
                0,
                1,
                1,
                false,
                new[] { _objectiveA, _objectiveB });

            _missionCompletedCount = 0;
            _completedMissionId = string.Empty;

            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
            _missionService.Shutdown();
            EventBusTestUtility.Reset();

            Object.DestroyImmediate(_mission);
            Object.DestroyImmediate(_objectiveA);
            Object.DestroyImmediate(_objectiveB);
        }

        [Test]
        public void Mission_Should_Complete_When_All_Objectives_Are_Completed()
        {
            _missionService.StartMission(_mission);

            _objectiveService.AddProgress("objective_a", 1);

            Assert.AreEqual(0, _missionCompletedCount);
            Assert.IsTrue(_missionService.HasActiveMission);

            _objectiveService.AddProgress("objective_b", 1);

            Assert.AreEqual(1, _missionCompletedCount);
            Assert.AreEqual("mission_001", _completedMissionId);
            Assert.IsFalse(_missionService.HasActiveMission);
        }

        private void OnMissionCompleted(MissionCompletedEvent e)
        {
            _missionCompletedCount++;
            _completedMissionId = e.MissionId;
        }
    }
}