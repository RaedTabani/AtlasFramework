using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Objectives.Definitions;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Services;
using NUnit.Framework;
using UnityEngine;

namespace DeviGames.Atlas.Core.Objectives.Tests
{
    public class ObjectiveServiceTests
    {
        private ObjectiveService _service;
        private ObjectiveDefinition _objective;

        private int _startedCount;
        private int _progressChangedCount;
        private int _completedCount;

        [SetUp]
        public void Setup()
        {
            EventBusTestUtility.Reset();

            _service = new ObjectiveService();
            _objective = ScriptableObject.CreateInstance<ObjectiveDefinition>();

            _objective.Editor_InitializeForTests(
                "test_objective_001",
                "Test Objective",
                "Test Description",
                3);

            _startedCount = 0;
            _progressChangedCount = 0;
            _completedCount = 0;

            EventBus.Subscribe<ObjectiveStartedEvent>(_ => _startedCount++);
            EventBus.Subscribe<ObjectiveProgressChangedEvent>(_ => _progressChangedCount++);
            EventBus.Subscribe<ObjectiveCompletedEvent>(_ => _completedCount++);
        }

        [TearDown]
        public void TearDown()
        {
            EventBusTestUtility.Reset();

            if (_objective != null)
                Object.DestroyImmediate(_objective);
        }

        [Test]
        public void StartObjective_Should_Create_State_And_Publish_Event()
        {
            _service.StartObjective(_objective);

            Assert.IsTrue(_service.States.ContainsKey("test_objective_001"));
            Assert.AreEqual(1, _startedCount);
        }

        [Test]
        public void AddProgress_Should_Publish_Progress_Event()
        {
            _service.StartObjective(_objective);

            _service.AddProgress("test_objective_001", 1);

            Assert.AreEqual(1, _progressChangedCount);
        }

        [Test]
        public void AddProgress_When_Target_Reached_Should_Publish_Completed_Event()
        {
            _service.StartObjective(_objective);

            _service.AddProgress("test_objective_001", 3);

            Assert.IsTrue(_service.IsCompleted("test_objective_001"));
            Assert.AreEqual(1, _completedCount);
        }

        [Test]
        public void AddProgress_After_Completed_Should_Not_Publish_Again()
        {
            _service.StartObjective(_objective);

            _service.AddProgress("test_objective_001", 3);
            _service.AddProgress("test_objective_001", 1);

            Assert.AreEqual(1, _completedCount);
        }

        [Test]
        public void AddProgress_With_Invalid_Id_Should_Do_Nothing()
        {
            _service.AddProgress("missing", 1);

            Assert.AreEqual(0, _progressChangedCount);
            Assert.AreEqual(0, _completedCount);
        }
    }
}