using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Core.Unlocks.Services;

namespace DeviGames.Atlas.Core.Unlocks.Installation
{
    public sealed class UnlockSaveIntegrationInstaller :
        IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IUnlockService unlockService = services.Resolve<IUnlockService>();
            SaveService saveService = services.Resolve<SaveService>();
            SaveParticipantCollection participants = services.Resolve<SaveParticipantCollection>();

            var coordinator = new UnlockSaveCoordinator(
                unlockService,
                saveService);

            services.Register(coordinator);
            participants.Add(coordinator);
        }
    }
}