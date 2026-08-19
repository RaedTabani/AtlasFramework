using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Rewards.Services;
using DeviGames.Atlas.Gameplay.Inventory.Rewards;
using DeviGames.Atlas.Gameplay.Inventory.Services;

namespace DeviGames.Atlas.Gameplay.Inventory.Tests
{
    public sealed class InventoryRewardIntegrationTests
    {
        private InventoryService _inventoryService;
        private RewardService _rewardService;

        [SetUp]
        public void SetUp()
        {
            _inventoryService = new InventoryService();

            var handlerRegistry = new RewardHandlerRegistry();

            handlerRegistry.Register(
                new InventoryRewardHandler(_inventoryService));

            _rewardService = new RewardService(handlerRegistry);

            _rewardService.Register(
                new RewardDefinition(
                    id: "reward.playground.golden-key",
                    type: InventoryRewardHandler.RewardType,
                    targetId: "golden_key",
                    amount: 1));

            _rewardService.AddMissionReward(
                new MissionRewardBinding(
                    "mission.playground.escape",
                    "reward.playground.golden-key"));

            _rewardService.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _rewardService.Shutdown();
        }

        [Test]
        public void MissionCompleted_GrantsInventoryReward()
        {
            Assert.That(
                _inventoryService.GetQuantity("golden_key"),
                Is.EqualTo(0));

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.playground.escape"));

            Assert.That(
                _inventoryService.GetQuantity("golden_key"),
                Is.EqualTo(1));
        }

        [Test]
        public void DifferentMissionCompleted_DoesNotGrantInventoryReward()
        {
            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.playground.other"));

            Assert.That(
                _inventoryService.GetQuantity("golden_key"),
                Is.EqualTo(0));
        }

        [Test]
        public void MissionCompleted_GrantsConfiguredAmount()
        {
            _rewardService.Register(
                new RewardDefinition(
                    id: "reward.playground.keys",
                    type: InventoryRewardHandler.RewardType,
                    targetId: "key",
                    amount: 3));

            _rewardService.AddMissionReward(
                new MissionRewardBinding(
                    "mission.playground.bonus",
                    "reward.playground.keys"));

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.playground.bonus"));

            Assert.That(
                _inventoryService.GetQuantity("key"),
                Is.EqualTo(3));
        }
    }
}