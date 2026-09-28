using System;

namespace DeviGames.Atlas.Gameplay.Inventory.Models
{
    public sealed class ItemDefinition
    {
        public string Id { get; }

        public string DisplayName { get; }

        public ItemDefinition(
            string id,
            string displayName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Item ID cannot be empty.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Item display name cannot be empty.",
                    nameof(displayName));
            }

            Id =
                id;

            DisplayName =
                displayName;
        }
    }
}