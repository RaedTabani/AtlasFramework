using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Save.Installation
{
    public sealed class SaveInstaller : IAtlasInstaller
    {
        private readonly ISaveStorage _storage;

        public SaveInstaller(
            ISaveStorage storage)
        {
            _storage =
                storage
                ?? throw new ArgumentNullException(nameof(storage));
        }

        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            var saveService =
                new SaveService(
                    _storage);

            var participants =
                new SaveParticipantCollection();

            var coordinator =
                new SaveGameCoordinator(
                    participants);

            services.Register(
                saveService);

            services.Register(
                participants);

            services.Register(
                coordinator);
        }
    }
}