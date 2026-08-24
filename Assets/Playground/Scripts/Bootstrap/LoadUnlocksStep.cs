using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Unlocks.Services;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class LoadUnlocksStep :
        IBootstrapStep
    {
        public string Name =>
            "Load Unlocks";

        public async Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            UnlockSaveCoordinator coordinator =
                context.Services.Resolve<UnlockSaveCoordinator>();

            await coordinator.LoadAsync();
        }
    }
}