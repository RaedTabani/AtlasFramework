using System;

using DeviGames.Atlas.Core.Rewards.Interfaces;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Unlocks.Interfaces;

namespace DeviGames.Atlas.Core.Unlocks.Rewards
{
    public sealed class UnlockRewardHandler : IRewardHandler
    {
        public const string RewardType = "unlock";

        private readonly IUnlockService _unlockService;

        public string Type => RewardType;

        public UnlockRewardHandler(IUnlockService unlockService)
        {
            _unlockService = unlockService ?? throw new ArgumentNullException(nameof(unlockService));
        }

        public bool Grant(RewardDefinition reward)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(nameof(reward));
            }

            return _unlockService.Unlock(reward.TargetId);
        }
    }
}