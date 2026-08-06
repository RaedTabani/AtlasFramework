using System;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Objectives.Collections;
using DeviGames.Atlas.Core.Objectives.Factories;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Services.Interfaces;

namespace DeviGames.Atlas.Core.Objectives.Installation
{
    public sealed class ObjectiveInstaller :
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

            var objectiveCollection =
                new ObjectiveCollection();

            var objectiveFactory =
                new ObjectiveFactory();

            var objectiveService =
                new ObjectiveService(
                    objectiveFactory,
                    objectiveCollection);

            services.Register<IObjectiveCollection>(
                objectiveCollection);

            services.Register<IObjectiveFactory>(
                objectiveFactory);

            services.Register(
                objectiveService);
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            bool hasCollection =
                services.TryResolve<IObjectiveCollection>(
                    out _);

            bool hasFactory =
                services.TryResolve<IObjectiveFactory>(
                    out _);

            bool hasService =
                services.TryResolve<ObjectiveService>(
                    out _);

            if (hasCollection ||
                hasFactory ||
                hasService)
            {
                throw new InvalidOperationException(
                    "Atlas Objectives are already installed " +
                    "or the installation is incomplete.");
            }
        }
    }
}