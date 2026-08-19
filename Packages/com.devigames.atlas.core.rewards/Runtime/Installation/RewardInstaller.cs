using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Rewards.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Rewards.Installation
{
    public sealed class RewardInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            var handlerRegistry = new RewardHandlerRegistry();
            var rewardService = new RewardService(handlerRegistry);

            services.Register(handlerRegistry);
            services.Register(rewardService);
        }
    }
}