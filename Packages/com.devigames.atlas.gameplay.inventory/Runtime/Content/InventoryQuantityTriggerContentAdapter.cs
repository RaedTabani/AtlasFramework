using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;

namespace DeviGames.Atlas.Gameplay.Inventory.Content
{
    public sealed class InventoryQuantityTriggerContentAdapter :
        ITriggerContentConditionAdapter
    {
        public const string ConditionType =
            "inventory.quantity";

        public string Type =>
            ConditionType;

        public ITriggerConditionDefinition CreateDefinition(
            TriggerContentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            InventoryQuantityConditionContentData condition =
                data.InventoryQuantity;

            if (condition == null)
            {
                throw new InvalidOperationException(
                    $"Trigger '{data.Id}' requires " +
                    "InventoryQuantity condition data.");
            }

            return new InventoryQuantityConditionDefinition(
                itemId:
                    condition.ItemId,
                requiredQuantity:
                    condition.RequiredQuantity);
        }
    }
}