using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Rewards;

namespace DeviGames.Atlas.Gameplay.Currency.Installation
{
    public sealed class CurrencyRewardIntegrationInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            ICurrencyService currencyService =
                services.Resolve<ICurrencyService>();

            RewardHandlerRegistry rewardHandlers =
                services.Resolve<RewardHandlerRegistry>();

            rewardHandlers.Register(
                new CurrencyRewardHandler(
                    currencyService));
        }
    }
}