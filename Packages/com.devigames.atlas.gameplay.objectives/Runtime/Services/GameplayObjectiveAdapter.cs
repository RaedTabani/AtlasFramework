using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.Objectives.Models;

namespace DeviGames.Atlas.Gameplay.Objectives.Services
{
    public sealed class GameplayObjectiveAdapter :
        IInitializable,
        IShutdownable
    {
        private readonly ObjectiveService
            _objectiveService;

        private readonly List<ItemCollectedObjectiveBinding>
            _itemBindings = new();

        private readonly List<DoorOpenedObjectiveBinding>
            _doorBindings = new();

        private readonly List<AreaEnteredObjectiveBinding>
            _areaBindings = new();

        public GameplayObjectiveAdapter(
            ObjectiveService objectiveService)
        {
            _objectiveService =
                objectiveService
                ?? throw new ArgumentNullException(
                    nameof(objectiveService));
        }

        public void AddItemBinding(
            ItemCollectedObjectiveBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(
                    nameof(binding));
            }

            _itemBindings.Add(
                binding);
        }

        public void AddDoorBinding(
            DoorOpenedObjectiveBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(
                    nameof(binding));
            }

            _doorBindings.Add(
                binding);
        }

        public void AddAreaBinding(
            AreaEnteredObjectiveBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(
                    nameof(binding));
            }

            _areaBindings.Add(
                binding);
        }

        public void Initialize()
        {
            EventBus.Subscribe<ItemCollectedEvent>(
                OnItemCollected);

            EventBus.Subscribe<DoorOpenedEvent>(
                OnDoorOpened);

            EventBus.Subscribe<AreaEnteredEvent>(
                OnAreaEntered);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<ItemCollectedEvent>(
                OnItemCollected);

            EventBus.Unsubscribe<DoorOpenedEvent>(
                OnDoorOpened);

            EventBus.Unsubscribe<AreaEnteredEvent>(
                OnAreaEntered);
        }

        private void OnItemCollected(
            ItemCollectedEvent eventData)
        {
            for (int index = 0;
                 index < _itemBindings.Count;
                 index++)
            {
                ItemCollectedObjectiveBinding binding =
                    _itemBindings[index];

                if (!string.Equals(
                        binding.ItemId,
                        eventData.ItemId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                _objectiveService.AddProgress(
                    binding.ObjectiveId,
                    binding.ProgressAmount);
            }
        }

        private void OnDoorOpened(
            DoorOpenedEvent eventData)
        {
            for (int index = 0;
                 index < _doorBindings.Count;
                 index++)
            {
                DoorOpenedObjectiveBinding binding =
                    _doorBindings[index];

                if (!string.Equals(
                        binding.DoorId,
                        eventData.DoorId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                _objectiveService.AddProgress(
                    binding.ObjectiveId,
                    binding.ProgressAmount);
            }
        }

        private void OnAreaEntered(
            AreaEnteredEvent eventData)
        {
            for (int index = 0;
                 index < _areaBindings.Count;
                 index++)
            {
                AreaEnteredObjectiveBinding binding =
                    _areaBindings[index];

                if (!string.Equals(
                        binding.AreaId,
                        eventData.AreaId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                _objectiveService.AddProgress(
                    binding.ObjectiveId,
                    binding.ProgressAmount);
            }
        }
    }
}