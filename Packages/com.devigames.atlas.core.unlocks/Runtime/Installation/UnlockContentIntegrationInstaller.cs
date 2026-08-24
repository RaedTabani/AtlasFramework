using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Unlocks.Content;
using DeviGames.Atlas.Core.Unlocks.Interfaces;

namespace DeviGames.Atlas.Core.Unlocks.Installation
{
    public sealed class UnlockContentIntegrationInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IUnlockService unlockService = services.Resolve<IUnlockService>();
            ContentPackageConsumerCollection consumers = services.Resolve<ContentPackageConsumerCollection>();

            var contentInstaller =
                new UnlockContentInstaller(unlockService);

            services.Register(contentInstaller);
            consumers.Add(contentInstaller);
        }
    }
}