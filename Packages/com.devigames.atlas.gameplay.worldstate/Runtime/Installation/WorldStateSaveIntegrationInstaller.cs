using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.WorldState.Interfaces;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Installation
{
    public sealed class WorldStateSaveIntegrationInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IWorldStateService worldStateService =
                services.Resolve<IWorldStateService>();

            SaveService saveService =
                services.Resolve<SaveService>();

            SaveParticipantCollection participants =
                services.Resolve<SaveParticipantCollection>();

            var coordinator =
                new WorldStateSaveCoordinator(
                    worldStateService,
                    saveService);

            services.Register(
                coordinator);

            participants.Add(
                coordinator);
        }
    }
}