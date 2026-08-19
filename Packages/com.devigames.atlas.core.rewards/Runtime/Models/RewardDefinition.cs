using System;

namespace DeviGames.Atlas.Core.Rewards.Models
{
    [Serializable]
    public sealed class RewardDefinition
    {
        public string Id { get; }

        public string Type { get; }

        public string TargetId { get; }

        public int Amount { get; }

        public RewardDefinition(
            string id,
            string type,
            string targetId,
            int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Reward ID cannot be empty.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(
                    "Reward type cannot be empty.",
                    nameof(type));
            }

            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Reward target ID cannot be empty.",
                    nameof(targetId));
            }

            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount));
            }

            Id = id;
            Type = type;
            TargetId = targetId;
            Amount = amount;
        }
    }
}