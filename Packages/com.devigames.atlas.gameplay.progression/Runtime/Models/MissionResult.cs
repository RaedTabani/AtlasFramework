using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.Progression.Models
{
    public sealed class MissionResult
    {
        private readonly List<MissionRewardResult> _rewards =
            new();

        public string MissionId { get; }

        public bool Completed { get; }

        public IReadOnlyList<MissionRewardResult> Rewards =>
            _rewards;

        public int RewardCount =>
            _rewards.Count;

        public MissionResult(
            string missionId,
            bool completed)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                throw new ArgumentException(
                    "Mission ID cannot be empty.",
                    nameof(missionId));
            }

            MissionId = missionId;
            Completed = completed;
        }

        internal void AddReward(
            MissionRewardResult reward)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(
                    nameof(reward));
            }

            _rewards.Add(
                reward);
        }
    }
}