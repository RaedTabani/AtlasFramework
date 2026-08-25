using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class LoadGameStep :
        IBootstrapStep
    {
        public string Name => "Load Game";

        public async Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            SaveGameCoordinator coordinator =
                context.Services.Resolve<SaveGameCoordinator>();

            await coordinator.LoadAsync();
        }
    }
}