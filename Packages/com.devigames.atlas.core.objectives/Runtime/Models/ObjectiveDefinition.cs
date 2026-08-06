using System;

namespace DeviGames.Atlas.Core.Objectives.Models
{
    public sealed class ObjectiveDefinition
    {
        public string Id { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public int TargetValue { get; }

        public bool IsOptional { get; }

        public bool IsHidden { get; }

        public ObjectiveDefinition(
            string id,
            string displayName,
            string description,
            int targetValue,
            bool isOptional = false,
            bool isHidden = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Objective ID cannot be empty.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Objective display name cannot be empty.",
                    nameof(displayName));
            }

            if (targetValue < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(targetValue),
                    targetValue,
                    "Objective target value must be at least one.");
            }

            Id = id;
            DisplayName = displayName;
            Description = description ?? string.Empty;
            TargetValue = targetValue;
            IsOptional = isOptional;
            IsHidden = isHidden;
        }
    }
}