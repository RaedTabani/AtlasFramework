using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Services;

using DeviGames.Atlas.Gameplay.WorldState.Content;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Installation
{
    public sealed class WorldStateContentIntegrationInstaller :
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

            GameplayWorldStateAdapter adapter =
                services.Resolve<
                    GameplayWorldStateAdapter>();

            var contentInstaller =
                new WorldStateContentInstaller(
                    adapter);

            services.Register(
                contentInstaller);

            services.Resolve<
                    ContentPackageConsumerCollection>()
                .Add(
                    contentInstaller);
        }
    }
}