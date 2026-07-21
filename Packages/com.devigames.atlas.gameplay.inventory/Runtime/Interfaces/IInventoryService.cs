using System.Collections.Generic;
using DeviGames.Atlas.Gameplay.Inventory.Models;

namespace DeviGames.Atlas.Gameplay.Inventory.Interfaces
{
    public interface IInventoryService
    {
        IReadOnlyCollection<string> ItemIds { get; }
        int ItemCount { get; }

        bool Contains(string itemId);
        int GetQuantity(string itemId);
        bool Remove(string itemId);

        InventoryData CreateSnapshot();
        void LoadInventory(InventoryData data);
    }
}