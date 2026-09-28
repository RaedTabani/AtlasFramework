using System;
using System.Collections.Generic;

using DeviGames.Atlas.Gameplay.Inventory.Models;

namespace DeviGames.Atlas.Gameplay.Inventory.Registry
{
    public sealed class ItemDefinitionRegistry
    {
        private readonly Dictionary<string, ItemDefinition>
            _definitions =
                new(StringComparer.Ordinal);

        public void Register(
            ItemDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (!_definitions.TryAdd(
                    definition.Id,
                    definition))
            {
                throw new InvalidOperationException(
                    $"Item definition '{definition.Id}' is already registered.");
            }
        }

        public bool TryGet(
            string itemId,
            out ItemDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(
                    itemId))
            {
                definition =
                    null;

                return false;
            }

            return _definitions.TryGetValue(
                itemId,
                out definition);
        }

        public ItemDefinition Get(
            string itemId)
        {
            if (string.IsNullOrWhiteSpace(
                    itemId))
            {
                throw new ArgumentException(
                    "Item ID cannot be empty.",
                    nameof(itemId));
            }

            if (!_definitions.TryGetValue(
                    itemId,
                    out ItemDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Item definition '{itemId}' is not registered.");
            }

            return definition;
        }
    }
}