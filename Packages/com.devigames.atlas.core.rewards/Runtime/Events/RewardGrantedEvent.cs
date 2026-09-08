namespace DeviGames.Atlas.Core.Rewards.Events
{
    public readonly struct RewardGrantedEvent
    {
        public string RewardId { get; }

        public string RewardType { get; }

        public RewardGrantedEvent(
            string rewardId,
            string rewardType)
        {
            RewardId =
                rewardId;

            RewardType =
                rewardType;
        }
    }
}