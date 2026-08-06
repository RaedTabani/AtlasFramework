using System;

namespace DeviGames.Atlas.Gameplay.Objectives.Bindings
{
    public sealed class ItemCollectedObjectiveBinding
    {
        public string ObjectiveId { get; }

        public string ItemId { get; }

        public int ProgressPerItem { get; }

        public ItemCollectedObjectiveBinding(
            string objectiveId,
            string itemId,
            int progressPerItem = 1)
        {
            if (string.IsNullOrWhiteSpace(objectiveId))
            {
                throw new ArgumentException(
                    "Objective ID cannot be empty.",
                    nameof(objectiveId));
            }

            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException(
                    "Item ID cannot be empty.",
                    nameof(itemId));
            }

            if (progressPerItem < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(progressPerItem));
            }

            ObjectiveId = objectiveId;
            ItemId = itemId;
            ProgressPerItem = progressPerItem;
        }
    }
}