using System;
using NUnit.Framework;

using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Gameplay.Currency.Rewards;
using DeviGames.Atlas.Gameplay.Currency.Services;

namespace DeviGames.Atlas.Gameplay.Currency.Tests
{
    public sealed class CurrencyRewardHandlerTests
    {
        private CurrencyService _currencyService;
        private CurrencyRewardHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _currencyService =
                new CurrencyService();

            _handler =
                new CurrencyRewardHandler(
                    _currencyService);
        }

        [Test]
        public void Type_ReturnsCurrency()
        {
            Assert.That(
                _handler.Type,
                Is.EqualTo(
                    CurrencyRewardHandler.RewardType));

            Assert.That(
                _handler.Type,
                Is.EqualTo("currency"));
        }

        [Test]
        public void Grant_ValidReward_AddsCurrency()
        {
            var reward =
                new RewardDefinition(
                    id: "reward.coins.100",
                    type: CurrencyRewardHandler.RewardType,
                    targetId: "coins",
                    amount: 100);

            bool result =
                _handler.Grant(
                    reward);

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _currencyService.GetBalance("coins"),
                Is.EqualTo(100));
        }

        [Test]
        public void Grant_MultipleRewards_AccumulatesCurrency()
        {
            var first =
                new RewardDefinition(
                    id: "reward.coins.100",
                    type: CurrencyRewardHandler.RewardType,
                    targetId: "coins",
                    amount: 100);

            var second =
                new RewardDefinition(
                    id: "reward.coins.50",
                    type: CurrencyRewardHandler.RewardType,
                    targetId: "coins",
                    amount: 50);

            _handler.Grant(
                first);

            _handler.Grant(
                second);

            Assert.That(
                _currencyService.GetBalance("coins"),
                Is.EqualTo(150));
        }

        [Test]
        public void Grant_DifferentCurrencies_UpdatesCorrectBalances()
        {
            var coins =
                new RewardDefinition(
                    id: "reward.coins.100",
                    type: CurrencyRewardHandler.RewardType,
                    targetId: "coins",
                    amount: 100);

            var gems =
                new RewardDefinition(
                    id: "reward.gems.5",
                    type: CurrencyRewardHandler.RewardType,
                    targetId: "gems",
                    amount: 5);

            _handler.Grant(
                coins);

            _handler.Grant(
                gems);

            Assert.That(
                _currencyService.GetBalance("coins"),
                Is.EqualTo(100));

            Assert.That(
                _currencyService.GetBalance("gems"),
                Is.EqualTo(5));
        }

        [Test]
        public void RewardDefinition_ZeroAmount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                    new RewardDefinition(
                        id: "reward.coins.invalid",
                        type: CurrencyRewardHandler.RewardType,
                        targetId: "coins",
                        amount: 0));
        }
    }
}