using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Content;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Sequence.Installation
{
    public sealed class SequenceContentIntegrationInstaller :
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

            SequenceDefinitionCollection sequenceCollection =
                services.Resolve<SequenceDefinitionCollection>();

            ContentPackageConsumerCollection consumers =
                services.Resolve<ContentPackageConsumerCollection>();

            var converterRegistry =
                new SequenceStepContentConverterRegistry();

            converterRegistry.Register(
                new ShowTextStepContentConverter());

            converterRegistry.Register(
                new WaitForContinueStepContentConverter());

            var contentInstaller =
                new SequenceContentInstaller(
                    sequenceCollection,
                    converterRegistry);

            consumers.Add(
                contentInstaller);
        }
    }
}