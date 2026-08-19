using DeviGames.Atlas.Core.Rewards.Models;

namespace DeviGames.Atlas.Core.Rewards.Interfaces
{
    public interface IRewardHandler
    {
        string Type { get; }

        bool Grant(RewardDefinition reward);
    }
}