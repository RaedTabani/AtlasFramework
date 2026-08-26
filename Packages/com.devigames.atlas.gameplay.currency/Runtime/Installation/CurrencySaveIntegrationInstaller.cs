using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Services;

namespace DeviGames.Atlas.Gameplay.Currency.Installation
{
    public sealed class CurrencySaveIntegrationInstaller :
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

            SaveService saveService =
                services.Resolve<SaveService>();

            SaveParticipantCollection participants =
                services.Resolve<SaveParticipantCollection>();

            var coordinator =
                new CurrencySaveCoordinator(
                    currencyService,
                    saveService);

            services.Register(
                coordinator);

            participants.Add(
                coordinator);
        }
    }
}