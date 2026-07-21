using System;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Gameplay.Inventory.Triggers
{
    public sealed class InventoryQuantityConditionDefinition :
        ITriggerConditionDefinition
    {
        public const string ConditionType =
            "inventory.quantity";

        public string Type =>
            ConditionType;

        public string ItemId { get; }

        public int RequiredQuantity { get; }

        public InventoryQuantityConditionDefinition(
            string itemId,
            int requiredQuantity)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException(
                    "Item ID cannot be empty.",
                    nameof(itemId));
            }

            if (requiredQuantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requiredQuantity));
            }

            ItemId = itemId;
            RequiredQuantity = requiredQuantity;
        }
    }
}