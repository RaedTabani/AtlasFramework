using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.WorldState.Interfaces;
using DeviGames.Atlas.Gameplay.WorldState.Models;

namespace DeviGames.Atlas.Gameplay.WorldState.Services
{
    public sealed class GameplayWorldStateAdapter :
        IInitializable,
        IShutdownable
    {
        private readonly IWorldStateService
            _worldStateService;

        private readonly List<
            DoorOpenedWorldStateBinding>
            _doorOpenedBindings =
                new();

        public GameplayWorldStateAdapter(
            IWorldStateService worldStateService)
        {
            _worldStateService =
                worldStateService
                ?? throw new ArgumentNullException(
                    nameof(worldStateService));
        }

        public void Initialize()
        {
            EventBus.Subscribe<DoorOpenedEvent>(
                OnDoorOpened);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<DoorOpenedEvent>(
                OnDoorOpened);
        }

        public void AddDoorOpenedBinding(
            DoorOpenedWorldStateBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(
                    nameof(binding));
            }

            _doorOpenedBindings.Add(
                binding);
        }

        private void OnDoorOpened(
            DoorOpenedEvent eventData)
        {
            for (int index = 0;
                 index < _doorOpenedBindings.Count;
                 index++)
            {
                DoorOpenedWorldStateBinding binding =
                    _doorOpenedBindings[index];

                if (!string.Equals(
                        binding.DoorId,
                        eventData.DoorId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                _worldStateService.Set(
                    binding.StateKey,
                    binding.Value);
            }
        }
    }
}