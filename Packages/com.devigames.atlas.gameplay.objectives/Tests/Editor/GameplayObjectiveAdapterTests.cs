using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Objectives.Definitions;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using NUnit.Framework;
using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Objectives.Tests
{
    public sealed class GameplayObjectiveAdapterTests
    {
        private ObjectiveService _objectiveService;
        private GameplayObjectiveAdapter _adapter;
        private ObjectiveDefinition _objective;

        [SetUp]
        public void Setup()
        {
            EventBusTestUtility.Reset();

            _objectiveService = new ObjectiveService();
            _adapter = new GameplayObjectiveAdapter(_objectiveService);
            _adapter.Initialize();

            _objective = ScriptableObject.CreateInstance<ObjectiveDefinition>();

            _objective.Editor_InitializeForTests(
                "collect_golden_key",
                "Collect the Golden Key",
                "Find and collect the golden key.",
                1,
                GameplayObjectiveAdapter.ItemCollectedSignal,
                "golden_key");

            _objectiveService.StartObjective(_objective);
        }

        [TearDown]
        public void TearDown()
        {
            _adapter.Shutdown();
            EventBusTestUtility.Reset();

            if (_objective != null)
                Object.DestroyImmediate(_objective);
        }

        [Test]
        public void Matching_ItemCollectedEvent_Should_Complete_Objective()
        {
            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            Assert.IsTrue(
                _objectiveService.IsCompleted("collect_golden_key"));
        }

        [Test]
        public void NonMatching_ItemCollectedEvent_Should_Not_Update_Objective()
        {
            EventBus.Publish(new ItemCollectedEvent("silver_key"));

            Assert.IsFalse(
                _objectiveService.IsCompleted("collect_golden_key"));
        }

        [Test]
        public void Matching_Event_Should_Respect_Target_Value()
        {
            Object.DestroyImmediate(_objective);

            _objective = ScriptableObject.CreateInstance<ObjectiveDefinition>();

            _objective.Editor_InitializeForTests(
                "collect_three_coins",
                "Collect Three Coins",
                "Collect three coins.",
                3,
                GameplayObjectiveAdapter.ItemCollectedSignal,
                "coin");

            _objectiveService.Reset();
            _objectiveService.StartObjective(_objective);

            EventBus.Publish(new ItemCollectedEvent("coin"));
            EventBus.Publish(new ItemCollectedEvent("coin"));

            Assert.IsFalse(
                _objectiveService.IsCompleted("collect_three_coins"));

            EventBus.Publish(new ItemCollectedEvent("coin"));

            Assert.IsTrue(
                _objectiveService.IsCompleted("collect_three_coins"));
        }

        [Test]
        public void Adapter_Shutdown_Should_Stop_Reacting_To_Events()
        {
            _adapter.Shutdown();

            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            Assert.IsFalse(
                _objectiveService.IsCompleted("collect_golden_key"));
        }
    }
}