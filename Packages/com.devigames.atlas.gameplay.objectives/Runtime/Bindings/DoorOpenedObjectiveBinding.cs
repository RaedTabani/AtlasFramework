using System;

namespace DeviGames.Atlas.Gameplay.Objectives.Bindings
{
    public sealed class DoorOpenedObjectiveBinding
    {
        public string ObjectiveId { get; }

        public string DoorId { get; }

        public int ProgressAmount { get; }

        public DoorOpenedObjectiveBinding(
            string objectiveId,
            string doorId,
            int progressAmount = 1)
        {
            if (string.IsNullOrWhiteSpace(objectiveId))
            {
                throw new ArgumentException(
                    "Objective ID cannot be empty.",
                    nameof(objectiveId));
            }

            if (string.IsNullOrWhiteSpace(doorId))
            {
                throw new ArgumentException(
                    "Door ID cannot be empty.",
                    nameof(doorId));
            }

            if (progressAmount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(progressAmount));
            }

            ObjectiveId = objectiveId;
            DoorId = doorId;
            ProgressAmount = progressAmount;
        }
    }
}