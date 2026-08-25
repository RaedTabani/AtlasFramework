using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Progress.Services;

namespace DeviGames.Atlas.Core.Progress.Installation
{
    public sealed class ProgressSaveIntegrationInstaller :
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

            MissionProgressService progressService =
                services.Resolve<MissionProgressService>();

            SaveService saveService =
                services.Resolve<SaveService>();

            SaveParticipantCollection participants =
                services.Resolve<SaveParticipantCollection>();

            var coordinator =
                new ProgressSaveCoordinator(
                    progressService,
                    saveService);

            services.Register(coordinator);
            participants.Add(coordinator);
        }
    }
}