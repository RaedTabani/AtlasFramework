using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Events;
using DeviGames.Atlas.Gameplay.Progression.Models;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Tests
{
    public sealed class MissionResultServiceTests
    {
        private MissionResultService _service;

        [SetUp]
        public void SetUp()
        {
            _service =
                new MissionResultService();

            _service.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Shutdown();
        }

        [Test]
        public void NewService_HasNoResult()
        {
            Assert.That(
                _service.HasResult,
                Is.False);

            Assert.That(
                _service.CurrentResult,
                Is.Null);
        }

        [Test]
        public void MissionCompleted_CreatesResult()
        {
            CompleteMission(
                "mission.test");

            Assert.That(
                _service.HasResult,
                Is.True);

            Assert.That(
                _service.CurrentResult,
                Is.Not.Null);
        }

        [Test]
        public void MissionCompleted_ResultContainsMissionId()
        {
            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void MissionCompleted_ResultIsCompleted()
        {
            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.Completed,
                Is.True);
        }

        [Test]
        public void MissionCompleted_ResultInitiallyHasNoRewards()
        {
            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _service.CurrentResult.Rewards,
                Is.Empty);
        }

        [Test]
        public void TryGetResult_WhenResultExists_ReturnsResult()
        {
            CompleteMission(
                "mission.test");

            bool found =
                _service.TryGetResult(
                    out MissionResult result);

            Assert.That(
                found,
                Is.True);

            Assert.That(
                result,
                Is.SameAs(
                    _service.CurrentResult));
        }

        [Test]
        public void TryGetResult_WhenNoResultExists_ReturnsFalse()
        {
            bool found =
                _service.TryGetResult(
                    out MissionResult result);

            Assert.That(
                found,
                Is.False);

            Assert.That(
                result,
                Is.Null);
        }

        [Test]
        public void Clear_WhenResultExists_RemovesResult()
        {
            CompleteMission(
                "mission.test");

            _service.Clear();

            Assert.That(
                _service.HasResult,
                Is.False);

            Assert.That(
                _service.CurrentResult,
                Is.Null);
        }

        [Test]
        public void MissionCompleted_WhenResultAlreadyExists_ReplacesResult()
        {
            CompleteMission(
                "mission.first");

            MissionResult firstResult =
                _service.CurrentResult;

            CompleteMission(
                "mission.second");

            MissionResult secondResult =
                _service.CurrentResult;

            Assert.That(
                secondResult,
                Is.Not.SameAs(
                    firstResult));

            Assert.That(
                secondResult.MissionId,
                Is.EqualTo(
                    "mission.second"));
        }

        [Test]
        public void RewardGranted_AfterMissionCompleted_AddsRewardToResult()
        {
            CompleteMission(
                "mission.test");

            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    1));
        }

        [Test]
        public void RewardGranted_AfterMissionCompleted_ResultContainsCorrectRewardData()
        {
            CompleteMission(
                "mission.test");

            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            MissionRewardResult reward =
                _service.CurrentResult.Rewards[0];

            Assert.That(
                reward.RewardId,
                Is.EqualTo(
                    "reward.coins"));

            Assert.That(
                reward.Type,
                Is.EqualTo(
                    "currency"));

            Assert.That(
                reward.TargetId,
                Is.EqualTo(
                    "coins"));

            Assert.That(
                reward.Amount,
                Is.EqualTo(
                    100));
        }

        [Test]
        public void RewardGranted_BeforeMissionCompleted_IsAppliedWhenResultIsCreated()
        {
            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            Assert.That(
                _service.HasResult,
                Is.False);

            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _service.CurrentResult.Rewards[0].RewardId,
                Is.EqualTo(
                    "reward.coins"));
        }

        [Test]
        public void MultipleRewards_AfterMissionCompleted_AllAreAddedToResult()
        {
            CompleteMission(
                "mission.test");

            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            GrantReward(
                "mission.test",
                "reward.item",
                "inventory",
                "item.potion",
                2);

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    2));

            Assert.That(
                _service.CurrentResult.Rewards[0].RewardId,
                Is.EqualTo(
                    "reward.coins"));

            Assert.That(
                _service.CurrentResult.Rewards[1].RewardId,
                Is.EqualTo(
                    "reward.item"));
        }

        [Test]
        public void MultipleRewards_BeforeMissionCompleted_AllAreAppliedToResult()
        {
            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            GrantReward(
                "mission.test",
                "reward.item",
                "inventory",
                "item.potion",
                2);

            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    2));

            Assert.That(
                _service.CurrentResult.Rewards[0].RewardId,
                Is.EqualTo(
                    "reward.coins"));

            Assert.That(
                _service.CurrentResult.Rewards[1].RewardId,
                Is.EqualTo(
                    "reward.item"));
        }

        [Test]
        public void RewardGranted_ForDifferentMission_DoesNotLeakIntoCurrentResult()
        {
            CompleteMission(
                "mission.a");

            GrantReward(
                "mission.b",
                "reward.b",
                "currency",
                "coins",
                50);

            Assert.That(
                _service.CurrentResult.MissionId,
                Is.EqualTo(
                    "mission.a"));

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void PendingReward_ForDifferentMission_IsNotAppliedToWrongResult()
        {
            GrantReward(
                "mission.b",
                "reward.b",
                "currency",
                "coins",
                50);

            CompleteMission(
                "mission.a");

            Assert.That(
                _service.CurrentResult.MissionId,
                Is.EqualTo(
                    "mission.a"));

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void PendingReward_IsPreservedUntilMatchingMissionCompletes()
        {
            GrantReward(
                "mission.b",
                "reward.b",
                "currency",
                "coins",
                50);

            CompleteMission(
                "mission.a");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));

            CompleteMission(
                "mission.b");

            Assert.That(
                _service.CurrentResult.MissionId,
                Is.EqualTo(
                    "mission.b"));

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _service.CurrentResult.Rewards[0].RewardId,
                Is.EqualTo(
                    "reward.b"));
        }

        [Test]
        public void Clear_RemovesPendingRewards()
        {
            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            _service.Clear();

            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void Clear_RemovesResultAndPendingRewards()
        {
            CompleteMission(
                "mission.a");

            GrantReward(
                "mission.a",
                "reward.a",
                "currency",
                "coins",
                100);

            GrantReward(
                "mission.b",
                "reward.b",
                "inventory",
                "item.potion",
                1);

            _service.Clear();

            Assert.That(
                _service.HasResult,
                Is.False);

            Assert.That(
                _service.CurrentResult,
                Is.Null);

            CompleteMission(
                "mission.b");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void Shutdown_UnsubscribesFromMissionCompletedEvent()
        {
            _service.Shutdown();

            CompleteMission(
                "mission.test");

            Assert.That(
                _service.HasResult,
                Is.False);

            _service.Initialize();
        }

        [Test]
        public void Shutdown_UnsubscribesFromMissionRewardGrantedEvent()
        {
            _service.Shutdown();

            GrantReward(
                "mission.test",
                "reward.coins",
                "currency",
                "coins",
                100);

            _service.Initialize();

            CompleteMission(
                "mission.test");

            Assert.That(
                _service.CurrentResult.RewardCount,
                Is.EqualTo(
                    0));
        }

        private static void CompleteMission(
            string missionId)
        {
            EventBus.Publish(
                new MissionCompletedEvent(
                    missionId));
        }

        private static void GrantReward(
            string missionId,
            string rewardId,
            string type,
            string targetId,
            int amount)
        {
            EventBus.Publish(
                new MissionRewardGrantedEvent(
                    missionId,
                    rewardId,
                    type,
                    targetId,
                    amount));
        }
    }
}