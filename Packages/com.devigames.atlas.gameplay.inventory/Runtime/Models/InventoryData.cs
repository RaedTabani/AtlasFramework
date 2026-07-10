using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.Inventory.Models
{
    [Serializable]
    public sealed class InventoryData
    {
        public List<string> ItemIds = new();

        public bool Contains(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return false;

            return ItemIds.Contains(itemId);
        }

        public bool Add(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return false;

            if (ItemIds.Contains(itemId))
                return false;

            ItemIds.Add(itemId);
            return true;
        }

        public bool Remove(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return false;

            return ItemIds.Remove(itemId);
        }
    }
}