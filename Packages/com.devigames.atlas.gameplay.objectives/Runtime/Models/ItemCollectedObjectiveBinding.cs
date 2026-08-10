using System;

namespace DeviGames.Atlas.Gameplay.Objectives.Models
{
    public sealed class ItemCollectedObjectiveBinding
    {
        public string ObjectiveId { get; }

        public string ItemId { get; }

        public int ProgressAmount { get; }

        public ItemCollectedObjectiveBinding(
            string objectiveId,
            string itemId,
            int progressAmount = 1)
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

            if (progressAmount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(progressAmount));
            }

            ObjectiveId = objectiveId;
            ItemId = itemId;
            ProgressAmount = progressAmount;
        }
    }
}