using UnityEngine;
using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Services;
using DeviGames.Atlas.Core.Unlocks.Rewards;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Rewards.Services;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Registry;

using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Tests
{
    public class MissionAvailabilityServiceTests 
    {

        private IMissionAvailabilityService _availabilityService;
        private IUnlockService _unlockService;
        private MissionCollection _missionCollection;
        private RewardService _rewardService;

        [SetUp]
        public void SetUp()
        {
            _unlockService = new UnlockService();

            _missionCollection = new MissionCollection();

            _missionCollection.Add(
                CreateMission(
                    "mission.chapter-01"));

            _missionCollection.Add(
                CreateMission(
                    "mission.chapter-02"));

            _availabilityService =
                new MissionAvailabilityService(
                    _missionCollection,
                    _unlockService);

            var rewardHandlerRegistry =
                new RewardHandlerRegistry();

            rewardHandlerRegistry.Register(
                new UnlockRewardHandler(
                    _unlockService));

            _rewardService =
                new RewardService(
                    rewardHandlerRegistry);

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
        public void IsAvailable_ExistingAndUnlockedMission_ReturnsTrue()
        {
            _unlockService.Unlock(
                "mission.chapter-01");

            bool available =
                _availabilityService.IsAvailable(
                    "mission.chapter-01");

            Assert.That(available, Is.True);
        }

        [Test]
        public void IsAvailable_ExistingButLockedMission_ReturnsFalse()
        {
            bool available =
                _availabilityService.IsAvailable(
                    "mission.chapter-02");

            Assert.That(available, Is.False);
        }

        [Test]
        public void IsAvailable_UnknownMission_ReturnsFalse()
        {
            _unlockService.Unlock(
                "mission.unknown");

            bool available =
                _availabilityService.IsAvailable(
                    "mission.unknown");

            Assert.That(available, Is.False);
        }

        [Test]
        public void CompletingChapterOne_MakesChapterTwoAvailable()
        {
            Assert.That(
                _availabilityService.IsAvailable(
                    "mission.chapter-02"),
                Is.False);

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.chapter-01"));

            Assert.That(
                _availabilityService.IsAvailable(
                    "mission.chapter-02"),
                Is.True);
        }

        private static MissionRuntime CreateMission(
            string missionId)
        {
            var definition =
                new MissionDefinition(
                    id: missionId,
                    displayName: missionId,
                    description: "",
                    objectiveIds: new[]
                    {
                        $"{missionId}.objective"
                    });

            return new MissionRuntime(
                definition);
        }
    }
}
