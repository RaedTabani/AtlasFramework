using System;

namespace DeviGames.Atlas.Gameplay.Objectives.Models
{
    public sealed class AreaEnteredObjectiveBinding
    {
        public string ObjectiveId { get; }

        public string AreaId { get; }

        public int ProgressAmount { get; }

        public AreaEnteredObjectiveBinding(
            string objectiveId,
            string areaId,
            int progressAmount = 1)
        {
            if (string.IsNullOrWhiteSpace(objectiveId))
            {
                throw new ArgumentException(
                    "Objective ID cannot be empty.",
                    nameof(objectiveId));
            }

            if (string.IsNullOrWhiteSpace(areaId))
            {
                throw new ArgumentException(
                    "Area ID cannot be empty.",
                    nameof(areaId));
            }

            if (progressAmount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(progressAmount));
            }

            ObjectiveId = objectiveId;
            AreaId = areaId;
            ProgressAmount = progressAmount;
        }
    }
}