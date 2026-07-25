using System;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;

namespace DeviGames.Atlas.Gameplay.Inventory.Triggers
{
    public sealed class InventoryQuantityConditionFactory :
        ITriggerConditionFactory
    {
        public string Type =>
            InventoryQuantityConditionDefinition.ConditionType;
        private readonly IInventoryService _inventoryService;

        public InventoryQuantityConditionFactory(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService ??
                throw new ArgumentNullException(
                    nameof(inventoryService));
        }
        public ITriggerCondition Create(
            ITriggerConditionDefinition definition,
            TriggerBuildContext context)
        {
            if (definition is not
                InventoryQuantityConditionDefinition
                inventoryDefinition)
            {
                throw new ArgumentException(
                    $"Expected a definition of type " +
                    $"'{nameof(InventoryQuantityConditionDefinition)}', " +
                    $"but received '{definition?.GetType().Name ?? "null"}'.",
                    nameof(definition));
            }

            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            return new InventoryQuantityCondition(
                _inventoryService,
                inventoryDefinition.ItemId,
                inventoryDefinition.RequiredQuantity);
        }
    }
}