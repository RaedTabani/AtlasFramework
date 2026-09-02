using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Sequence.Installation
{
    public sealed class SequenceInstaller :
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

            var sequenceCollection =
                new SequenceDefinitionCollection();

            services.Register(
                sequenceCollection);
        }
    }
}