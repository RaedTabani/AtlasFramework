using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Triggers.Content;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Triggers.Installation
{
    public sealed class TriggerContentIntegrationInstaller :
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

            TriggerContentInstaller contentInstaller =
                services.Resolve<
                    TriggerContentInstaller>();

            ContentPackageConsumerCollection consumers =
                services.Resolve<
                    ContentPackageConsumerCollection>();

            consumers.Add(
                contentInstaller);
        }
    }
}