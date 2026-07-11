using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Gameplay.Events;

namespace DeviGames.Atlas.Gameplay.Objectives.Services
{
    public sealed class GameplayObjectiveAdapter :
        IInitializable,
        IShutdownable
    {
        public const string ItemCollectedSignal = "item_collected";
        public const string DoorOpenedSignal = "door_opened";
        public const string AreaEnteredSignal = "area_entered";

        private readonly ObjectiveService _objectiveService;

        public GameplayObjectiveAdapter(ObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Initialize()
        {
            EventBus.Subscribe<ItemCollectedEvent>(OnItemCollected);
            EventBus.Subscribe<DoorOpenedEvent>(OnDoorOpened);
            EventBus.Subscribe<AreaEnteredEvent>(OnAreaEntered);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<ItemCollectedEvent>(OnItemCollected);
            EventBus.Unsubscribe<DoorOpenedEvent>(OnDoorOpened);
            EventBus.Unsubscribe<AreaEnteredEvent>(OnAreaEntered);
        }

        private void OnItemCollected(ItemCollectedEvent e)
        {
            _objectiveService.ProcessSignal(
                new ObjectiveSignal(
                    ItemCollectedSignal,
                    e.ItemId));
        }

        private void OnDoorOpened(DoorOpenedEvent e)
        {
            _objectiveService.ProcessSignal(
                new ObjectiveSignal(
                    DoorOpenedSignal,
                    e.DoorId));
        }

        private void OnAreaEntered(AreaEnteredEvent e)
        {
            _objectiveService.ProcessSignal(
                new ObjectiveSignal(
                    AreaEnteredSignal,
                    e.AreaId));
        }
    }
}