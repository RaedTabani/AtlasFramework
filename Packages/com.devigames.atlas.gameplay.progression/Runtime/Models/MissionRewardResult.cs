namespace DeviGames.Atlas.Gameplay.Progression.Models
{
    public sealed class MissionRewardResult
    {
        public string RewardId { get; }

        public string Type { get; }

        public string TargetId { get; }

        public int Amount { get; }

        public MissionRewardResult(
            string rewardId,
            string type,
            string targetId,
            int amount)
        {
            RewardId = rewardId;
            Type = type;
            TargetId = targetId;
            Amount = amount;
        }
    }
}