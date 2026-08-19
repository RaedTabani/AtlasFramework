using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;

using DeviGames.Atlas.Gameplay.WorldState.Services;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class LoadWorldStateStep :
        IBootstrapStep
    {
        public string Name =>
            "Load World State";

        public async Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            WorldStateSaveCoordinator coordinator =
                context.Services.Resolve<
                    WorldStateSaveCoordinator>();

            await coordinator.LoadAsync();
        }
    }
}