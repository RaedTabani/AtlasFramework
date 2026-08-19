using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Save.Services;

using DeviGames.Atlas.Gameplay.WorldState.Interfaces;
using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Atlas.Gameplay.WorldState.Installation
{
    public sealed class WorldStateInstaller :
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

            var worldStateService =
                new WorldStateService();

            var adapter =
                new GameplayWorldStateAdapter(
                    worldStateService);

            services.Register<IWorldStateService>(
                worldStateService);

            services.Register(
                adapter);



        }
    }
}