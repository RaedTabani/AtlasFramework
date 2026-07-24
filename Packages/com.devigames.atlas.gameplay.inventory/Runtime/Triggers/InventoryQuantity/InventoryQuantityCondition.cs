using System;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;

namespace DeviGames.Atlas.Gameplay.Inventory.Triggers
{
    public sealed class InventoryQuantityCondition :
        ITriggerCondition
    {
        private readonly IInventoryService _inventory;
        private readonly string _itemId;
        private readonly int _requiredQuantity;

        public InventoryQuantityCondition(IInventoryService inventory,string itemId,int requiredQuantity)
        {
            _inventory =
                inventory
                ?? throw new ArgumentNullException(
                    nameof(inventory));

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

            _itemId = itemId;
            _requiredQuantity = requiredQuantity;
        }

        public bool Evaluate(TriggerContext context)
        {
            int quantity = _inventory.GetQuantity(_itemId);

            return quantity >= _requiredQuantity;
        }
    }
}