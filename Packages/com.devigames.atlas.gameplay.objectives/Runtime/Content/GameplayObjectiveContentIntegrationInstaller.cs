using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Objectives.Services;

namespace DeviGames.Atlas.Gameplay.Objectives.Content
{
    public sealed class GameplayObjectiveContentIntegrationInstaller :
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

            GameplayObjectiveAdapter adapter =
                services.Resolve<GameplayObjectiveAdapter>();

            var contentInstaller =
                new GameplayObjectiveContentInstaller(
                    adapter);
            
            services.Register(
                contentInstaller);
        }
    }
}