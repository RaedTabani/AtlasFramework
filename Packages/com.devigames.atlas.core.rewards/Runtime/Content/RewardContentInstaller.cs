using System;

using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Services;

namespace DeviGames.Atlas.Core.Rewards.Content
{
    public sealed class RewardContentInstaller : IContentPackageConsumer
    {
        private readonly RewardService _rewardService;

        public int Order => 200;

        public RewardContentInstaller(RewardService rewardService)
        {
            _rewardService = rewardService ?? throw new ArgumentNullException(nameof(rewardService));
        }

        public void Install(ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            InstallRewards(package);
            InstallMissionBindings(package);
        }

        private void InstallRewards(ContentPackageData package)
        {
            foreach (RewardContentData data in package.Rewards)
            {
                _rewardService.Register(
                    new RewardDefinition(
                        id: data.Id,
                        type: data.Type,
                        targetId: data.TargetId,
                        amount: data.Amount));
            }
        }

        private void InstallMissionBindings(ContentPackageData package)
        {
            foreach (MissionRewardBindingData data in package.MissionRewardBindings)
            {
                _rewardService.AddMissionReward(
                    new MissionRewardBinding(
                        data.MissionId,
                        data.RewardId));
            }
        }
    }
}