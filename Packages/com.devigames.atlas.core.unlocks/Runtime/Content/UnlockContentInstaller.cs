using System;

using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Unlocks.Interfaces;

namespace DeviGames.Atlas.Core.Unlocks.Content
{
    public sealed class UnlockContentInstaller :
        IContentPackageConsumer
    {
        private readonly IUnlockService _unlockService;

        public int Order => 300;

        public UnlockContentInstaller(
            IUnlockService unlockService)
        {
            _unlockService = unlockService ?? throw new ArgumentNullException(nameof(unlockService));
        }

        public void Install(ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            foreach (string unlockId in package.InitialUnlocks)
            {
                _unlockService.Unlock(unlockId);
            }
        }
    }
}