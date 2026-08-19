using System;

namespace DeviGames.Atlas.Core.Rewards.Models
{
    public sealed class MissionRewardBinding
    {
        public string MissionId { get; }
        public string RewardId { get; }

        public MissionRewardBinding(string missionId, string rewardId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                throw new ArgumentException(
                    "Mission ID cannot be empty.",
                    nameof(missionId));
            }

            if (string.IsNullOrWhiteSpace(rewardId))
            {
                throw new ArgumentException(
                    "Reward ID cannot be empty.",
                    nameof(rewardId));
            }

            MissionId = missionId;
            RewardId = rewardId;
        }
    }
}