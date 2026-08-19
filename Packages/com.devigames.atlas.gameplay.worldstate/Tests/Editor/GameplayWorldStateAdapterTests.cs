using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.WorldState.Models;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Tests
{
    public sealed class GameplayWorldStateAdapterTests
    {
        private WorldStateService
            _worldStateService;

        private GameplayWorldStateAdapter
            _adapter;

        [SetUp]
        public void SetUp()
        {
            _worldStateService =
                new WorldStateService();

            _adapter =
                new GameplayWorldStateAdapter(
                    _worldStateService);

            _adapter.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _adapter.Shutdown();
        }

        [Test]
        public void DoorOpened_MatchingBinding_SetsWorldState()
        {
            _adapter.AddDoorOpenedBinding(
                new DoorOpenedWorldStateBinding(
                    doorId:
                        "door.exit",
                    stateKey:
                        "world.exit.opened"));

            EventBus.Publish(
                new DoorOpenedEvent(
                    "door.exit"));

            Assert.That(
                _worldStateService.Get(
                    "world.exit.opened"),
                Is.True);
        }

        [Test]
        public void DoorOpened_NonMatchingBinding_DoesNotSetWorldState()
        {
            _adapter.AddDoorOpenedBinding(
                new DoorOpenedWorldStateBinding(
                    doorId:
                        "door.exit",
                    stateKey:
                        "world.exit.opened"));

            EventBus.Publish(
                new DoorOpenedEvent(
                    "door.other"));

            Assert.That(
                _worldStateService.Contains(
                    "world.exit.opened"),
                Is.False);
        }

        [Test]
        public void DoorOpened_BindingCanSetFalse()
        {
            _worldStateService.Set(
                "world.exit.opened",
                true);

            _adapter.AddDoorOpenedBinding(
                new DoorOpenedWorldStateBinding(
                    doorId:
                        "door.reset",
                    stateKey:
                        "world.exit.opened",
                    value:
                        false));

            EventBus.Publish(
                new DoorOpenedEvent(
                    "door.reset"));

            Assert.That(
                _worldStateService.Get(
                    "world.exit.opened"),
                Is.False);
        }
    }
}