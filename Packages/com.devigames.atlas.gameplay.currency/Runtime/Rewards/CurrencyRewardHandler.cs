using System;

using DeviGames.Atlas.Core.Rewards.Interfaces;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;

namespace DeviGames.Atlas.Gameplay.Currency.Rewards
{
    public sealed class CurrencyRewardHandler :
        IRewardHandler
    {
        public const string RewardType =
            "currency";

        private readonly ICurrencyService
            _currencyService;

        public string Type =>
            RewardType;

        public CurrencyRewardHandler(
            ICurrencyService currencyService)
        {
            _currencyService =
                currencyService
                ?? throw new ArgumentNullException(nameof(currencyService));
        }

        public bool Grant(
            RewardDefinition reward)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(nameof(reward));
            }

            return _currencyService.Add(
                reward.TargetId,
                reward.Amount);
        }
    }
}