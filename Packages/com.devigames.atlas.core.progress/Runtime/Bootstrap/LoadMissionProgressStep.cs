using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Progress.Bootstrap
{
    public sealed class LoadMissionProgressStep :
        IBootstrapStep
    {
        public string Name =>
            "Load Mission Progress";

        public async Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ProgressSaveCoordinator coordinator =
                context.Services.Resolve<
                    ProgressSaveCoordinator>();

            await coordinator.LoadAsync();
        }
    }
}