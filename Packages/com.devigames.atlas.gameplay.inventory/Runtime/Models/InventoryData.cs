using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.Inventory.Models
{
    [Serializable]
    public sealed class InventoryData
    {
        public List<string> ItemIds = new();
        public List<int> Quantities = new();

        public bool Contains(string itemId)
        {
            return IndexOf(itemId) >= 0;
        }

        public int GetQuantity(string itemId)
        {
            var index = IndexOf(itemId);
            return index >= 0 ? Quantities[index] : 0;
        }

        /// <summary>
        /// Adds one or more units of an item. Returns true only when the item
        /// was not previously present (i.e. this is a brand-new stack) so callers
        /// can decide whether to fire a "first pickup" style event. Returns false
        /// when the item already existed and its quantity was simply incremented.
        /// </summary>
        public bool Add(string itemId, int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
                return false;

            var index = IndexOf(itemId);
            if (index >= 0)
            {
                Quantities[index] += amount;
                return false;
            }

            ItemIds.Add(itemId);
            Quantities.Add(amount);
            return true;
        }

        /// <summary>
        /// Removes one or more units of an item. Returns false if the item
        /// isn't present or there isn't enough quantity to remove. When the
        /// removed amount brings the stack to zero, the item is dropped entirely
        /// (use Contains afterward to detect this).
        /// </summary>
        public bool Remove(string itemId, int amount = 1)
        {
            if (amount <= 0)
                return false;

            var index = IndexOf(itemId);
            if (index < 0 || Quantities[index] < amount)
                return false;

            Quantities[index] -= amount;

            if (Quantities[index] <= 0)
            {
                ItemIds.RemoveAt(index);
                Quantities.RemoveAt(index);
            }

            return true;
        }

        private int IndexOf(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return -1;

            return ItemIds.IndexOf(itemId);
        }
    }
}