using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Loading;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class LoadPlaygroundContentStep :
        IBootstrapStep
    {
        public string Name =>
            "Load Playground Content";

        public async Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ContentPackageLoader loader =
                context.Services.Resolve<
                    ContentPackageLoader>();

            await loader.LoadAsync(
                "playground.chapter-01");
        }
    }
}