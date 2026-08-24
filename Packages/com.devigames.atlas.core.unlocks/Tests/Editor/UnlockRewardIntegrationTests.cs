using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Rewards.Services;
using DeviGames.Atlas.Core.Unlocks.Rewards;
using DeviGames.Atlas.Core.Unlocks.Services;

namespace DeviGames.Atlas.Core.Unlocks.Tests
{
    public sealed class UnlockRewardIntegrationTests
    {
        private UnlockService _unlockService;
        private RewardService _rewardService;

        [SetUp]
        public void SetUp()
        {
            _unlockService = new UnlockService();

            var handlerRegistry = new RewardHandlerRegistry();

            handlerRegistry.Register(
                new UnlockRewardHandler(_unlockService));

            _rewardService = new RewardService(handlerRegistry);

            _rewardService.Register(
                new RewardDefinition(
                    id: "reward.unlock.chapter-02",
                    type: UnlockRewardHandler.RewardType,
                    targetId: "mission.chapter-02",
                    amount: 1));

            _rewardService.AddMissionReward(
                new MissionRewardBinding(
                    "mission.chapter-01",
                    "reward.unlock.chapter-02"));

            _rewardService.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _rewardService.Shutdown();
        }

        [Test]
        public void MissionCompleted_GrantsUnlockReward()
        {
            Assert.That(
                _unlockService.IsUnlocked("mission.chapter-02"),
                Is.False);

            EventBus.Publish(
                new MissionCompletedEvent("mission.chapter-01"));

            Assert.That(
                _unlockService.IsUnlocked("mission.chapter-02"),
                Is.True);
        }
        [Test]
        public void Grant_SameUnlockTwice_SecondGrantReturnsFalse()
        {
            var service = new UnlockService();
            var handler = new UnlockRewardHandler(service);

            var reward = new RewardDefinition(
                id: "reward.unlock.chapter-02",
                type: UnlockRewardHandler.RewardType,
                targetId: "mission.chapter-02",
                amount: 1);

            bool first = handler.Grant(reward);
            bool second = handler.Grant(reward);

            Assert.That(first, Is.True);
            Assert.That(second, Is.False);
        }
    }
}