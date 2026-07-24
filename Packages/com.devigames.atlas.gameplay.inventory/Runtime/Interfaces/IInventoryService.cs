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

        bool Add(string itemId,int quantity);
        bool Remove(string itemId, int quantity);

        InventoryData CreateSnapshot();
        void LoadInventory(InventoryData data);
    }
}