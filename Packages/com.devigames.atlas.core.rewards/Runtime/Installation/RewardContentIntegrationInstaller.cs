using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Rewards.Content;
using DeviGames.Atlas.Core.Rewards.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Rewards.Installation
{
    public sealed class RewardContentIntegrationInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            RewardService rewardService = services.Resolve<RewardService>();
            ContentPackageConsumerCollection consumers = services.Resolve<ContentPackageConsumerCollection>();

            var contentInstaller = new RewardContentInstaller(rewardService);

            services.Register(contentInstaller);
            consumers.Add(contentInstaller);
        }
    }
}