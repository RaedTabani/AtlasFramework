using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Installation
{
    public sealed class ProgressionInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IMissionCollection missionCollection = services.Resolve<IMissionCollection>();
            IObjectiveCollection objectiveCollection = services.Resolve<IObjectiveCollection>();
            IUnlockService unlockService = services.Resolve<IUnlockService>();

            var availabilityService = new MissionAvailabilityService(missionCollection, unlockService);
            var sessionService = new MissionSessionService(missionCollection,objectiveCollection,availabilityService);
            var resultService = new MissionResultService();
            var objectiveProgressionService = new MissionObjectiveProgressionService(sessionService,missionCollection);

            services.Register<IMissionAvailabilityService>(availabilityService);
            services.Register<IMissionSessionService>(sessionService);
            services.Register<IMissionResultService>(resultService);
            services.Register<IMissionObjectiveProgressionService>(objectiveProgressionService);


        }
    }
}