using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Services;

namespace DeviGames.Atlas.Gameplay.Currency.Installation
{
    public sealed class CurrencyInstaller :
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

            var currencyService =
                new CurrencyService();

            services.Register<ICurrencyService>(
                currencyService);
        }
    }
}