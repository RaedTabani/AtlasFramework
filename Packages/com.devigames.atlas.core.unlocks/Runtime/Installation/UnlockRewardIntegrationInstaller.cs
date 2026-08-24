using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Rewards;

namespace DeviGames.Atlas.Core.Unlocks.Installation
{
    public sealed class UnlockRewardIntegrationInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IUnlockService unlockService = services.Resolve<IUnlockService>();
            RewardHandlerRegistry rewardHandlers = services.Resolve<RewardHandlerRegistry>();

            rewardHandlers.Register(
                new UnlockRewardHandler(unlockService));
        }
    }
}