using System;
using DeviGames.Atlas.Core.Objectives.Models;

namespace DeviGames.Atlas.Core.Objectives.Runtime
{
    public sealed class ObjectiveRuntime
    {
        public ObjectiveDefinition Definition { get; }
        public string Id => Definition.Id;
        public string DisplayName => Definition.DisplayName;
        public string Description => Definition.Description;

        public int CurrentValue { get; private set; }
        public int TargetValue => Definition.TargetValue;
        public bool IsCompleted =>  State == ObjectiveState.Completed;
        public ObjectiveState State { get; private set; }

        public float NormalizedProgress => (float)CurrentValue / TargetValue;

        public ObjectiveRuntime( ObjectiveDefinition definition)
        {
            Definition =
                definition
                ?? throw new ArgumentNullException(
                    nameof(definition));

            CurrentValue = 0;

            State =
                ObjectiveState.Active;
        }

        public ObjectiveUpdateResult AddProgress(
            int amount)
        {
            if (amount <= 0)
            {
                return ObjectiveUpdateResult.None;
            }

            if (IsCompleted)
            {
                return ObjectiveUpdateResult.None;
            }

            int previousValue =
                CurrentValue;

            CurrentValue =
                Math.Min(
                    CurrentValue + amount,
                    TargetValue);

            if (CurrentValue ==
                previousValue)
            {
                return ObjectiveUpdateResult.None;
            }

            if (CurrentValue >=
                TargetValue)
            {
                State =
                    ObjectiveState.Completed;

                return ObjectiveUpdateResult.Completed;
            }

            return ObjectiveUpdateResult.Progressed;
        }
    }
}