using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Serialization;
using DeviGames.Atlas.Core.Content.Validation;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Content.Installation
{
    public sealed class ContentInstaller :
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
            var consumers = new ContentPackageConsumerCollection();
            var parser = new ContentJsonParser();
            var validator = new ContentPackageValidator();

            var preflight =
                new ContentPackagePreflight(
                    services.Resolve<IObjectiveCollection>(),
                    services.Resolve<IMissionCollection>());

            var packageInstaller =
                new ContentPackageInstaller(
                    validator,
                    preflight,
                    services.Resolve<ObjectiveService>(),
                    services.Resolve<MissionService>());

            consumers.Add(packageInstaller);
            services.Register(
                parser);

            services.Register(
                validator);

            services.Register(
                preflight);
            services.Register(
                consumers);

            services.Register(
                packageInstaller);


        }
    }
}