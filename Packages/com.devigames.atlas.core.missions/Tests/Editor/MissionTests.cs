using NUnit.Framework;
using UnityEngine;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Definitions;
using DeviGames.Atlas.Core.Events;

namespace DeviGames.Atlas.Core.Missions.Tests
{
    public class MissionTests
    {
        private MissionService _missionService;
        private MissionDefinition _mockMission;
        
        private int _startedCount;
        private int _completedCount;
        private string _lastProcessedMissionId;

        
        [SetUp]
        public void Setup()
        {
            _startedCount = 0;
            _completedCount = 0;
            _lastProcessedMissionId = string.Empty;

            // 1. Initialize your State Architecture objects
            _missionService = new MissionService();
            
            // 2. Generate a clean Runtime-only scriptable object instance
            _mockMission = ScriptableObject.CreateInstance<MissionDefinition>();
            _mockMission.Editor_InitializeForTests(
                "test_mission_001",
                "Test Mission",
                "Test Description",
                "TestScene",
                0,
                1,
                1,
                false);
            
            // Using a bit of reflection hack or an initialization method if fields are fully protected, 
            // but for testing standard properties we look for matching events.
            
            // 3. Clear global static footprints via the Provider
            // (Adjust name to EventBusTestUtility or EventBusProvider based on your wrapper layout)
            EventBusTestUtility.Reset(); 
        }

        [TearDown]
        public void Teardown()
        {
            // Destroy memory footprint allocations of the test ScriptableObject
            Object.DestroyImmediate(_mockMission);
        }

        // --- Mission Manager Functional Tests ---

        [Test]
        public void StartMission_Should_SetActiveMission_And_Publish_MissionStartedEvent()
        {
            // Arrange
            EventBus.Subscribe<MissionStartedEvent>(OnMissionStarted);

            // Act
            _missionService.StartMission(_mockMission);

            // Assert
            Assert.IsTrue(_missionService.HasActiveMission);
            Assert.AreEqual(_mockMission, _missionService.CurrentMission);
            Assert.AreEqual(1, _startedCount);

            EventBus.Unsubscribe<MissionStartedEvent>(OnMissionStarted);
        }

        [Test]
        public void CompleteMission_Should_ClearActiveMission_And_Publish_MissionCompletedEvent()
        {
            // Arrange
            _missionService.StartMission(_mockMission);
            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);

            // Act
            _missionService.CompleteMission();

            // Assert
            Assert.IsFalse(_missionService.HasActiveMission);
            Assert.IsNull(_missionService.CurrentMission);
            Assert.AreEqual(1, _completedCount);

            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        [Test]
        public void CompleteMission_WhenNoActiveMission_Should_Not_PublishEvent()
        {
            // Arrange
            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);

            // Act
            _missionService.CompleteMission();

            // Assert
            Assert.AreEqual(0, _completedCount);

            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        [Test]
        public void AbortMission_Should_ClearActiveMission_Without_Publishing_CompletionEvent()
        {
            // Arrange
            _missionService.StartMission(_mockMission);
            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);

            // Act
            _missionService.AbortMission();

            // Assert
            Assert.IsFalse(_missionService.HasActiveMission);
            Assert.AreEqual(0, _completedCount);

            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        // --- Helper Event Handlers ---
        private void OnMissionStarted(MissionStartedEvent e)
        {
            _startedCount++;
            _lastProcessedMissionId = e.MissionId;
        }

        private void OnMissionCompleted(MissionCompletedEvent e)
        {
            _completedCount++;
            _lastProcessedMissionId = e.MissionId;
        }
    }
}