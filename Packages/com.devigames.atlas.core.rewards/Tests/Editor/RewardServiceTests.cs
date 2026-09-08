using System;

using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Events;
using DeviGames.Atlas.Core.Rewards.Interfaces;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Rewards.Services;

namespace DeviGames.Atlas.Core.Rewards.Tests
{
    public sealed class RewardServiceTests
    {
        private const string RewardType =
            "test";

        private RewardHandlerRegistry _handlerRegistry;
        private FakeRewardHandler _handler;
        private RewardService _service;

        private MissionRewardGrantedEvent
            _lastRewardGrantedEvent;

        private int _rewardGrantedEventCount;

        [SetUp]
        public void SetUp()
        {
            _rewardGrantedEventCount = 0;
            _lastRewardGrantedEvent = default;

            _handlerRegistry =
                new RewardHandlerRegistry();

            _handler =
                new FakeRewardHandler(
                    RewardType);

            _handlerRegistry.Register(
                _handler);

            _service =
                new RewardService(
                    _handlerRegistry);

            EventBus.Subscribe<MissionRewardGrantedEvent>(
                OnRewardGranted);

            _service.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Shutdown();

            EventBus.Unsubscribe<MissionRewardGrantedEvent>(
                OnRewardGranted);
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_PublishesRewardGrantedEvent()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _handler.GrantCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _rewardGrantedEventCount,
                Is.EqualTo(
                    1));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantFails_DoesNotPublishRewardGrantedEvent()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                false;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _handler.GrantCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _rewardGrantedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_EventContainsCorrectMissionId()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _lastRewardGrantedEvent.MissionId,
                Is.EqualTo(
                    "mission.a"));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_EventContainsCorrectRewardId()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _lastRewardGrantedEvent.RewardId,
                Is.EqualTo(
                    "reward.a"));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_EventContainsCorrectType()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _lastRewardGrantedEvent.Type,
                Is.EqualTo(
                    RewardType));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_EventContainsCorrectTargetId()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _lastRewardGrantedEvent.TargetId,
                Is.EqualTo(
                    "target.a"));
        }

        [Test]
        public void MissionCompleted_WhenRewardGrantSucceeds_EventContainsCorrectAmount()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _lastRewardGrantedEvent.Amount,
                Is.EqualTo(
                    5));
        }

        [Test]
        public void MissionCompleted_ForDifferentMission_DoesNotGrantReward()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.b"));

            Assert.That(
                _handler.GrantCallCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _rewardGrantedEventCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void MissionCompleted_WithMultipleRewards_GrantsAllMatchingRewards()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            RegisterMissionReward(
                "mission.a",
                "reward.b",
                "target.b",
                10);

            _handler.GrantResult =
                true;

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _handler.GrantCallCount,
                Is.EqualTo(
                    2));

            Assert.That(
                _rewardGrantedEventCount,
                Is.EqualTo(
                    2));
        }

        [Test]
        public void MissionCompleted_WhenRewardIsNotRegistered_Throws()
        {
            _service.AddMissionReward(
                new MissionRewardBinding(
                    "mission.a",
                    "reward.missing"));

            Assert.Throws<InvalidOperationException>(
                () =>
                    EventBus.Publish(
                        new MissionCompletedEvent(
                            "mission.a")));
        }

        [Test]
        public void Shutdown_UnsubscribesFromMissionCompletedEvent()
        {
            RegisterMissionReward(
                "mission.a",
                "reward.a",
                "target.a",
                5);

            _service.Shutdown();

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.a"));

            Assert.That(
                _handler.GrantCallCount,
                Is.EqualTo(
                    0));

            Assert.That(
                _rewardGrantedEventCount,
                Is.EqualTo(
                    0));

            _service.Initialize();
        }

        private void RegisterMissionReward(
            string missionId,
            string rewardId,
            string targetId,
            int amount)
        {
            var reward =
                new RewardDefinition(
                    rewardId,
                    RewardType,
                    targetId,
                    amount);

            _service.Register(
                reward);

            _service.AddMissionReward(
                new MissionRewardBinding(
                    missionId,
                    rewardId));
        }

        private void OnRewardGranted(
            MissionRewardGrantedEvent eventData)
        {
            _rewardGrantedEventCount++;

            _lastRewardGrantedEvent =
                eventData;
        }

        private sealed class FakeRewardHandler :
            IRewardHandler
        {
            public string Type { get; }

            public bool GrantResult { get; set; } =
                true;

            public int GrantCallCount { get; private set; }

            public RewardDefinition LastReward { get; private set; }

            public FakeRewardHandler(
                string type)
            {
                Type =
                    type;
            }

            public bool Grant(
                RewardDefinition reward)
            {
                GrantCallCount++;

                LastReward =
                    reward;

                return GrantResult;
            }
        }
    }
}