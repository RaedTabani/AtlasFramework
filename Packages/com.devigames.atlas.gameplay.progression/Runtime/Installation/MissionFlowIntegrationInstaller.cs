using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;

namespace DeviGames.Atlas.Gameplay.Progression.Installation
{
    public sealed class MissionFlowIntegrationInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            EnsureNotInstalled(
                services);

            IMissionSessionService sessionService =
                services.Resolve<IMissionSessionService>();

            IGameFlowService gameFlowService =
                services.Resolve<IGameFlowService>();

            IMissionResultService resultService =
                services.Resolve<IMissionResultService>();

            var coordinator =
                new MissionFlowCoordinator(
                    sessionService,
                    gameFlowService,
                    resultService);

            services.Register(
                coordinator);
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            if (services.TryResolve<MissionFlowCoordinator>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas Mission Flow integration is already installed.");
            }
        }
    }
}