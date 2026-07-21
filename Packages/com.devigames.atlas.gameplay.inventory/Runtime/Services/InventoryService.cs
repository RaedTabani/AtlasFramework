using System.Collections.Generic;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.Inventory.Events;
using DeviGames.Atlas.Gameplay.Inventory.Models;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
namespace DeviGames.Atlas.Gameplay.Inventory.Services
{
    public sealed class InventoryService : IInitializable, IShutdownable,IInventoryService
    {
        private InventoryData _data = new();

        public IReadOnlyCollection<string> ItemIds => _data.ItemIds;
        public int ItemCount => _data.ItemIds.Count;

        public void Initialize()
        {
            EventBus.Subscribe<ItemCollectedEvent>(OnItemCollected);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<ItemCollectedEvent>(OnItemCollected);
        }

        public bool Contains(string itemId)
        {
            return _data.Contains(itemId);
        }

        public int GetQuantity(string itemId)
        {
            return _data.GetQuantity(itemId);
        }

        public bool Remove(string itemId)
        {
            if (!_data.Remove(itemId))
                return false;

            EventBus.Publish(new ItemRemovedFromInventoryEvent(itemId));
            return true;
        }

        public InventoryData CreateSnapshot()
        {
            return new InventoryData
            {
                ItemIds = new List<string>(_data.ItemIds)
            };
        }

        public void LoadInventory(InventoryData data)
        {
            _data = data ?? new InventoryData();
        }

        private void OnItemCollected(ItemCollectedEvent e)
        {
            if (!_data.Add(e.ItemId))
                return;

            EventBus.Publish(new ItemAddedToInventoryEvent(e.ItemId));
        }
    }
}