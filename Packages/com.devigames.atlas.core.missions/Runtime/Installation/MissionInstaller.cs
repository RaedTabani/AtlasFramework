using System;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Factories;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Missions.Installation
{
    public sealed class MissionInstaller :
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

            var missionCollection =
                new MissionCollection();

            var missionFactory =
                new MissionFactory();

            var missionService =
                new MissionService(
                    missionFactory,
                    missionCollection);

            services.Register<IMissionCollection>(
                missionCollection);

            services.Register<IMissionFactory>(
                missionFactory);

            services.Register(
                missionService);
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            bool hasCollection =
                services.TryResolve<IMissionCollection>(
                    out _);

            bool hasFactory =
                services.TryResolve<IMissionFactory>(
                    out _);

            bool hasService =
                services.TryResolve<MissionService>(
                    out _);

            if (hasCollection ||
                hasFactory ||
                hasService)
            {
                throw new InvalidOperationException(
                    "Atlas Missions are already installed " +
                    "or the installation is incomplete.");
            }
        }
    }
}