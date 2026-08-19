using System;

namespace DeviGames.Atlas.Core.Rewards.Models
{
    public sealed class MissionRewardBinding
    {
        public string MissionId { get; }

        public RewardDefinition Reward { get; }

        public MissionRewardBinding(
            string missionId,
            RewardDefinition reward)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                throw new ArgumentException(
                    "Mission ID cannot be empty.",
                    nameof(missionId));
            }

            MissionId = missionId;

            Reward =
                reward
                ?? throw new ArgumentNullException(
                    nameof(reward));
        }
    }
}