namespace DeviGames.Atlas.Core.Rewards.Events
{
    public readonly struct MissionRewardGrantedEvent
    {
        public string MissionId { get; }

        public string RewardId { get; }

        public string Type { get; }

        public string TargetId { get; }

        public int Amount { get; }

        public MissionRewardGrantedEvent(
            string missionId,
            string rewardId,
            string type,
            string targetId,
            int amount)
        {
            MissionId = missionId;
            RewardId = rewardId;
            Type = type;
            TargetId = targetId;
            Amount = amount;
        }
    }
}