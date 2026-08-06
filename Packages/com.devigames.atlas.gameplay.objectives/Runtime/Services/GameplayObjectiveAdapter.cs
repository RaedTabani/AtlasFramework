using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.Objectives.Bindings;

namespace DeviGames.Atlas.Gameplay.Objectives.Services
{
    public sealed class GameplayObjectiveAdapter :
        IInitializable,
        IShutdownable
    {
        private readonly ObjectiveService _objectiveService;

        private readonly IReadOnlyList<
            ItemCollectedObjectiveBinding>
            _itemCollectedBindings;

        private readonly IReadOnlyList<
            DoorOpenedObjectiveBinding>
            _doorOpenedBindings;

        private readonly IReadOnlyList<
            AreaEnteredObjectiveBinding>
            _areaEnteredBindings;

        public GameplayObjectiveAdapter(
            ObjectiveService objectiveService,
            IReadOnlyList<ItemCollectedObjectiveBinding>
                itemCollectedBindings,
            IReadOnlyList<DoorOpenedObjectiveBinding>
                doorOpenedBindings,
            IReadOnlyList<AreaEnteredObjectiveBinding>
                areaEnteredBindings)
        {
            _objectiveService =
                objectiveService
                ?? throw new ArgumentNullException(
                    nameof(objectiveService));

            _itemCollectedBindings =
                itemCollectedBindings
                ?? throw new ArgumentNullException(
                    nameof(itemCollectedBindings));

            _doorOpenedBindings =
                doorOpenedBindings
                ?? throw new ArgumentNullException(
                    nameof(doorOpenedBindings));

            _areaEnteredBindings =
                areaEnteredBindings
                ?? throw new ArgumentNullException(
                    nameof(areaEnteredBindings));
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
                 index < _itemCollectedBindings.Count;
                 index++)
            {
                ItemCollectedObjectiveBinding binding =
                    _itemCollectedBindings[index];

                if (!string.Equals(
                        binding.ItemId,
                        eventData.ItemId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                int quantity =
                    GetCollectedQuantity(
                        eventData);

                _objectiveService.AddProgress(
                    binding.ObjectiveId,
                    binding.ProgressPerItem *
                    quantity);
            }
        }

        private void OnDoorOpened(
            DoorOpenedEvent eventData)
        {
            for (int index = 0;
                 index < _doorOpenedBindings.Count;
                 index++)
            {
                DoorOpenedObjectiveBinding binding =
                    _doorOpenedBindings[index];

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
                 index < _areaEnteredBindings.Count;
                 index++)
            {
                AreaEnteredObjectiveBinding binding =
                    _areaEnteredBindings[index];

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

        private static int GetCollectedQuantity(
            ItemCollectedEvent eventData)
        {
            // If ItemCollectedEvent already exposes Quantity,
            // replace this with:
            //
            // return eventData.Quantity;

            return 1;
        }
    }
}